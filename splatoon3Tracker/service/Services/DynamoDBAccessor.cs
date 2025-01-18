using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using Nagiyu.Common.DynamoDBManager.Services;
using Nagiyu.Splatoon3Tracker.Service.Consts;
using Nagiyu.Splatoon3Tracker.Service.Models.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nagiyu.Splatoon3Tracker.Service.Services
{
    public class DynamoDBAccessor : DynamoDBServiceBase
    {
        public const string TABLE_NAME = "Splatoon3Tracker";

        public const string INDEX_NAME = "KillRate-index-20250112";

        public DynamoDBAccessor(IConfiguration configuration)
        {
            var accessKey = configuration["Auth:Credentials:AWS:AccessKey"];
            var secretKey = configuration["Auth:Credentials:AWS:SecretKey"];
            var region = configuration["Auth:Credentials:AWS:Region"];
            var serviceUrl = configuration["Auth:Credentials:AWS:ServiceUrl"];

            InitializeClient(accessKey, secretKey, region, serviceUrl);
        }

        public async Task<List<KillRateRecord>> GetKillRates()
        {
            var items = await GetItems(TABLE_NAME, INDEX_NAME, nameof(Splatoon3Enums.RecordType), Splatoon3Enums.RecordType.KillRate.ToString());

            var killRates = new List<KillRateRecord>();

            foreach (var item in items)
            {
                killRates.Add(new KillRateRecord(item));
            }

            return killRates;
        }

        public async Task<string> AddKillRate(KillRateRecord killRate)
        {
            await Add(TABLE_NAME, killRate);

            return killRate.Id;
        }

        public async Task UpdateKillRate(string id, KillRateRecord killRate)
        {
            // user の全要素を properties に変換
            var properties = new Dictionary<string, AttributeValueUpdate>();

            // 全キーをループして properties に追加
            foreach (var property in killRate.GetType().GetProperties())
            {
                // DynamoDB には UserId は含めない
                if (property.Name == nameof(KillRateRecord.Id))
                {
                    continue;
                }

                // プロパティの値を取得
                var value = property.GetValue(killRate);

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

            await UpdateProperties(TABLE_NAME, nameof(KillRateRecord.Id), id, properties);
        }

        public async Task DeleteKillRate(string id)
        {
            await Delete(TABLE_NAME, nameof(KillRateRecord.Id), id);
        }
    }
}
