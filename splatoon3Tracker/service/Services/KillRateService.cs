using Nagiyu.Splatoon3Tracker.Service.Models;
using Nagiyu.Splatoon3Tracker.Service.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nagiyu.Splatoon3Tracker.Service.Services
{
    public class KillRateService
    {
        private readonly DynamoDBAccessor dynamoDBAccessor;

        public KillRateService(DynamoDBAccessor dynamoDBAccessor)
        {
            this.dynamoDBAccessor = dynamoDBAccessor;
        }

        public async Task<GetKillRatesResponse> GetKillRates()
        {
            var dbResulse = await dynamoDBAccessor.GetKillRates();

            var result = new GetKillRatesResponse()
            {
                KillRates = new List<KillRate>()
            };

            foreach (var dbKillRate in dbResulse)
            {
                result.KillRates.Add(JsonConvert.DeserializeObject<KillRate>(JsonConvert.SerializeObject(dbKillRate)));
            }

            return result;
        }

        public async Task<AddKillRateResponse> AddKillRate(KillRate killRate)
        {
            var dbKillRate = JsonConvert.DeserializeObject<Models.DB.KillRate>(JsonConvert.SerializeObject(killRate));

            var id = await dynamoDBAccessor.AddKillRate(dbKillRate);

            return new AddKillRateResponse { Id = id };
        }

        public async Task UpdateKillRate(string id, KillRate killRate)
        {
            var guid = Guid.Parse(id);

            var dbKillRate = JsonConvert.DeserializeObject<Models.DB.KillRate>(JsonConvert.SerializeObject(killRate));

            await dynamoDBAccessor.UpdateKillRate(guid, dbKillRate);
        }

        public async Task DeleteKillRate(string id)
        {
            var guid = Guid.Parse(id);

            await dynamoDBAccessor.DeleteKillRate(guid);
        }
    }
}
