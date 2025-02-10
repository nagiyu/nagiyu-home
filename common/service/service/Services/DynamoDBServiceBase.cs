using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
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
        /// DynamoDB コンテキスト
        /// </summary>
        private readonly DynamoDBContext context;

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

            // DynamoDBContextを初期化
            context = new DynamoDBContext(client);
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

            return (T)Activator.CreateInstance(typeof(T), item);
        }

        /// <summary>
        /// 全てのレコードを取得する
        /// </summary>
        /// <returns>レコードのリスト</returns>
        public async Task<List<T>> GetAllRecords<T>() where T : RecordBase
        {
            var request = new ScanRequest
            {
                TableName = TableName
            };

            var response = await client.ScanAsync(request);
            var items = response.Items;

            return items.Select(item => (T)Activator.CreateInstance(typeof(T), item)).ToList();
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
        public async Task UpdateRecord<T>(T record) where T : RecordBase
        {
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

                // プロパティの値が null の場合はスキップ
                if (value == null)
                {
                    continue;
                }

                // プロパティの値を AttributeValueUpdate に変換
                var attributeValue = new AttributeValue();

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String:
                        attributeValue.S = value.ToString();
                        break;
                    case TypeCode.Int32:
                        attributeValue.N = value.ToString();
                        break;
                    case TypeCode.Boolean:
                        attributeValue.BOOL = (bool)value;
                        break;
                    default:
                        continue; // サポートされていない型はスキップ
                }

                properties.Add(property.Name, new AttributeValueUpdate
                {
                    Action = AttributeAction.PUT,
                    Value = attributeValue
                });
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
