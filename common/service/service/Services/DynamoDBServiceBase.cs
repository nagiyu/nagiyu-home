using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Service.Models.DB;

namespace Nagiyu.Common.Service.Services
{
    /// <summary>
    /// DynamoDB サービスの基底クラス
    /// </summary>
    public abstract class DynamoDBServiceBase
    {
        /// <summary>
        /// テーブル名
        /// </summary>
        protected string TableName;

        /// <summary>
        /// DynamoDB クライアント
        /// </summary>
        private readonly AmazonDynamoDBClient client;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration">設定情報</param>
        public DynamoDBServiceBase(IConfiguration configuration)
        {
            var accessKey = configuration["Auth:Credentials:AWS:AccessKey"];
            var secretKey = configuration["Auth:Credentials:AWS:SecretKey"];
            var region = configuration["Auth:Credentials:AWS:Region"];
            var serviceUrl = configuration["Auth:Credentials:AWS:ServiceUrl"];

            AmazonDynamoDBConfig config;

            if (!string.IsNullOrEmpty(serviceUrl))
            {
                // ServiceURLが指定された場合
                config = new AmazonDynamoDBConfig
                {
                    ServiceURL = serviceUrl
                };
            }
            else
            {
                // ServiceURLが指定されていない場合
                config = new AmazonDynamoDBConfig
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(region) // AWS本番環境向け
                };
            }

            // DynamoDBクライアントを初期化
            client = new AmazonDynamoDBClient(accessKey, secretKey, config);
        }

        /// <summary>
        /// レコードを取得する
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>レコード</returns>
        public async Task<T> GetRecord<T>(Guid id) where T : RecordBase
        {
            var request = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {
                        nameof(RecordBase.Id),
                        new AttributeValue { S = id.ToString() }
                    }
                }
            };

            var response = await client.GetItemAsync(request);

            var item = response.Item;

            if (item.Count == 0)
            {
                return null;
            }

            return (T)Activator.CreateInstance(typeof(T), item);
        }

        /// <summary>
        /// レコードを取得する
        /// </summary>
        /// <param name="expressions">条件</param>
        /// <param name="limit">取得件数</param>
        /// <param name="startId">開始ID</param>
        /// <returns>レコードのリスト, 次のID</returns>
        public async Task<(List<T>, Guid?)> GetRecords<T>(Dictionary<string, string> expressions = null, int limit = 0, Guid? startId = null) where T : RecordBase
        {
            var request = new ScanRequest
            {
                TableName = TableName
            };

            if (expressions != null)
            {
                request.FilterExpression = string.Join(" AND ", expressions.Keys.Select(key => $"{key} = :{key}"));
                request.ExpressionAttributeValues = expressions.ToDictionary(kv => $":{kv.Key}", kv => new AttributeValue { S = kv.Value });
            }

            if (limit > 0)
            {
                request.Limit = limit;
            }

            if (startId != null)
            {
                request.ExclusiveStartKey = new Dictionary<string, AttributeValue>
                {
                    {
                        nameof(RecordBase.Id),
                        new AttributeValue { S = startId.ToString() }
                    }
                };
            }

            var response = await client.ScanAsync(request);
            var items = response.Items;

            Guid? nextId = null;
            if (response.LastEvaluatedKey.Count > 0 && Guid.TryParse(response.LastEvaluatedKey.FirstOrDefault().Value.S, out var id))
            {
                nextId = id;
            }

            return (items.Select(item => (T)Activator.CreateInstance(typeof(T), item)).ToList(), nextId);
        }

        /// <summary>
        /// レコードを追加する
        /// </summary>
        /// <param name="record">レコード</param>
        /// <returns>レコードのID</returns>
        public async Task<Guid> AddRecord<T>(T record) where T : RecordBase
        {
            var id = Guid.NewGuid();

            record.Id = id.ToString();
            record.CreatedAt = DateTime.Now.ToString();
            record.UpdatedAt = DateTime.Now.ToString();

            await UpdateItem(record);

            return id;
        }

        /// <summary>
        /// レコードを更新する
        /// </summary>
        /// <param name="record">レコード</param>
        public async Task UpdateRecord<T>(Guid id, T record) where T : RecordBase
        {
            record.Id = id.ToString();
            record.UpdatedAt = DateTime.Now.ToString();

            await UpdateItem(record);
        }

        /// <summary>
        /// レコードを削除する
        /// </summary>
        /// <param name="id">ID</param>
        public async Task DeleteRecord(Guid id)
        {
            var request = new DeleteItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {
                        nameof(RecordBase.Id),
                        new AttributeValue { S = id.ToString() }
                    }
                }
            };

            await client.DeleteItemAsync(request);
        }

        /// <summary>
        /// レコードを削除する
        /// </summary>
        /// <param name="record">レコード</param>
        public async Task DeleteRecord<T>(T record) where T : RecordBase
        {
            var request = new DeleteItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {
                        nameof(RecordBase.Id),
                        new AttributeValue { S = record.Id }
                    }
                }
            };

            await client.DeleteItemAsync(request);
        }

        /// <summary>
        /// 指定されたレコードを更新する
        /// </summary>
        /// <param name="record">レコード</param>
        private async Task UpdateItem<T>(T record) where T : RecordBase
        {
            var properties = new Dictionary<string, AttributeValueUpdate>();

            // 全キーをループして properties に追加
            foreach (var property in record.GetType().GetProperties())
            {
                // キーは含めない
                if (property.Name == nameof(RecordBase.Id))
                {
                    continue;
                }

                // プロパティの値を取得
                var value = property.GetValue(record);

                // プロパティの値が null の場合は削除
                if (value == null)
                {
                    properties.Add(property.Name, new AttributeValueUpdate
                    {
                        Action = AttributeAction.DELETE
                    });
                    continue;
                }

                // プロパティの値を AttributeValueUpdate に変換して properties に追加
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String:
                        properties.Add(property.Name, new AttributeValueUpdate
                        {
                            Action = AttributeAction.PUT,
                            Value = new AttributeValue { S = value.ToString() }
                        });
                        break;
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Double:
                        properties.Add(property.Name, new AttributeValueUpdate
                        {
                            Action = AttributeAction.PUT,
                            Value = new AttributeValue { N = value.ToString() }
                        });
                        break;
                    case TypeCode.Boolean:
                        properties.Add(property.Name, new AttributeValueUpdate
                        {
                            Action = AttributeAction.PUT,
                            Value = new AttributeValue { BOOL = (bool)value }
                        });
                        break;
                    case TypeCode.Object when value is List<string> list:
                        if (list.Count == 0)
                        {
                            properties.Add(property.Name, new AttributeValueUpdate
                            {
                                Action = AttributeAction.DELETE
                            });
                            continue; // 空のリストは削除
                        }
                        properties.Add(property.Name, new AttributeValueUpdate
                        {
                            Action = AttributeAction.PUT,
                            Value = new AttributeValue { SS = list }
                        });
                        break;
                    case TypeCode.Object when value is List<int> list:
                        if (list.Count == 0)
                        {
                            properties.Add(property.Name, new AttributeValueUpdate
                            {
                                Action = AttributeAction.DELETE
                            });
                            continue; // 空のリストは削除
                        }
                        properties.Add(property.Name, new AttributeValueUpdate
                        {
                            Action = AttributeAction.PUT,
                            Value = new AttributeValue { NS = list.Select(i => i.ToString()).ToList() }
                        });
                        break;
                    case TypeCode.Object when value is List<long> list:
                        if (list.Count == 0)
                        {
                            properties.Add(property.Name, new AttributeValueUpdate
                            {
                                Action = AttributeAction.DELETE
                            });
                            continue; // 空のリストは削除
                        }
                        properties.Add(property.Name, new AttributeValueUpdate
                        {
                            Action = AttributeAction.PUT,
                            Value = new AttributeValue { NS = list.Select(i => i.ToString()).ToList() }
                        });
                        break;
                    case TypeCode.Object when value is List<double> list:
                        if (list.Count == 0)
                        {
                            properties.Add(property.Name, new AttributeValueUpdate
                            {
                                Action = AttributeAction.DELETE
                            });
                            continue; // 空のリストは削除
                        }
                        properties.Add(property.Name, new AttributeValueUpdate
                        {
                            Action = AttributeAction.PUT,
                            Value = new AttributeValue { NS = list.Select(i => i.ToString()).ToList() }
                        });
                        break;
                    default:
                        System.Diagnostics.Trace.WriteLine($"Unsupported type: {value.GetType()}");
                        continue; // サポートされていない型はスキップ
                }
            }

            var updateRequest = new UpdateItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { nameof(RecordBase.Id), new AttributeValue { S = record.Id } }
                },
                AttributeUpdates = properties
            };

            await client.UpdateItemAsync(updateRequest);
        }
    }
}
