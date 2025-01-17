﻿using Microsoft.Extensions.Configuration;
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

            foreach (var killRate in killRates)
            {
                Debug.WriteLine($"{killRate.Id}, {killRate.RecordType}, {killRate.UserId}, {killRate.Battle}, {killRate.Rule}, {killRate.Weapon}, {killRate.Result}, {killRate.Kill}, {killRate.Assist}, {killRate.Death}, {killRate.Special}, {killRate.Date}, {killRate.MatchTime}");
            }
        }

        [TestMethod]
        public async Task AddKillRateTest()
        {
            var killRate = new KillRateRecord
            {
                Id = Guid.NewGuid().ToString(),
                RecordType = Splatoon3Enums.RecordType.KillRate.ToString(),
                UserId = Guid.NewGuid().ToString(),
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 2,
                Death = 3,
                Special = 4,
                Date = "2025-01-13 12:00",
                MatchTime = 180
            };

            var id = await dynamoDBAccessor.AddKillRate(killRate);

            Assert.IsNotNull(id);
        }

        [TestMethod]
        public async Task UpdateKillRateTest()
        {
            var killRate = new KillRateRecord
            {
                Id = Guid.NewGuid().ToString(),
                RecordType = Splatoon3Enums.RecordType.KillRate.ToString(),
                UserId = Guid.NewGuid().ToString(),
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 2,
                Death = 3,
                Special = 4,
                Date = "2025-01-13 12:00",
                MatchTime = 180
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
            var killRate = new KillRateRecord
            {
                Id = Guid.NewGuid().ToString(),
                RecordType = Splatoon3Enums.RecordType.KillRate.ToString(),
                UserId = Guid.NewGuid().ToString(),
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 2,
                Death = 3,
                Special = 4,
                Date = "2025-01-13 12:00",
                MatchTime = 180
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
