using Microsoft.Extensions.Configuration;
using Nagiyu.Common.DynamoDBManager.Services;
using Nagiyu.Splatoon3Tracker.Service.Models.DB;
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

        public async Task<List<KillRate>> GetKillRates()
        {
            var items = await GetItems(TABLE_NAME, INDEX_NAME, "RecordType", "KillRate");

            var killRates = new List<KillRate>();

            foreach (var item in items)
            {
                killRates.Add(new KillRate(item));
            }

            return killRates;
        }
    }
}
