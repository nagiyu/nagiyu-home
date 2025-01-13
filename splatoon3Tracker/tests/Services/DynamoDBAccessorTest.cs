using Microsoft.Extensions.Configuration;
using Nagiyu.Splatoon3Tracker.Service.Consts;
using Nagiyu.Splatoon3Tracker.Service.Models.DB;
using Nagiyu.Splatoon3Tracker.Service.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Nagiyu.Splatoon3Tracker.Service.Tests.Services
{
    [TestClass]
    public class DynamoDBAccessorTest
    {
        private readonly DynamoDBAccessor dynamoDBAccessor;

        private readonly IConfiguration configuration;

        public DynamoDBAccessorTest()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();

            dynamoDBAccessor = new DynamoDBAccessor(configuration);
        }

        [TestMethod]
        public async Task GetKillRatesTest()
        {
            var killRates = await dynamoDBAccessor.GetKillRates();

            Assert.IsNotNull(killRates);

            Debug.WriteLine(killRates.Count);
        }

        [TestMethod]
        public async Task AddKillRateTest()
        {
            var killRate = new KillRate
            {
                RecordType = Splatoon3Enums.RecordType.KillRate.ToString(),
                UserId = Guid.NewGuid(),
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 5,
                Death = 3,
                Special = 2,
                Date = new DateTime(2025, 1, 13, 12, 0, 0),
                MatchTime = new TimeSpan(0, 3, 0).TotalSeconds
            };

            var id = await dynamoDBAccessor.AddKillRate(killRate);

            Assert.IsNotNull(id);
        }

        [TestMethod]
        public async Task UpdateKillRateTest()
        {
            var killRate = new KillRate
            {
                RecordType = Splatoon3Enums.RecordType.KillRate.ToString(),
                UserId = Guid.NewGuid(),
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 5,
                Death = 3,
                Special = 2,
                Date = new DateTime(2025, 1, 13, 12, 0, 0),
                MatchTime = new TimeSpan(0, 3, 0).TotalSeconds
            };

            var id = await dynamoDBAccessor.AddKillRate(killRate);

            killRate.Id = id;

            killRate.Kill = 20;

            await dynamoDBAccessor.UpdateKillRate(id, killRate);

            var killRates = await dynamoDBAccessor.GetKillRates();
            var updatedKillRate = killRates.Find(k => k.Id == id);

            Assert.IsNotNull(updatedKillRate);
            Assert.AreEqual(20, updatedKillRate.Kill);
        }

        [TestMethod]
        public async Task DeleteKillRateTest()
        {
            var killRate = new KillRate
            {
                RecordType = Splatoon3Enums.RecordType.KillRate.ToString(),
                UserId = Guid.NewGuid(),
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 5,
                Death = 3,
                Special = 2,
                Date = new DateTime(2025, 1, 13, 12, 0, 0),
                MatchTime = new TimeSpan(0, 3, 0).TotalSeconds
            };

            var id = await dynamoDBAccessor.AddKillRate(killRate);

            killRate.Id = id;

            await dynamoDBAccessor.DeleteKillRate(id);

            var killRates = await dynamoDBAccessor.GetKillRates();
            var deletedKillRate = killRates.Find(k => k.Id == id);

            Assert.IsNull(deletedKillRate);
        }
    }
}
