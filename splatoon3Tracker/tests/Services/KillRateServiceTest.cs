using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Auth.Service.Interfaces;
using Nagiyu.Common.Auth.Service.Mocks;
using Nagiyu.Splatoon3Tracker.Service.Consts;
using Nagiyu.Splatoon3Tracker.Service.Models.Requests;
using Nagiyu.Splatoon3Tracker.Service.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Nagiyu.Splatoon3Tracker.Service.Tests.Services
{
    [TestClass]
    public class KillRateServiceTest
    {
        private readonly DynamoDBAccessor dynamoDBAccessor;
        private readonly IAuthService authService;
        private readonly KillRateService killRateService;

        private readonly IConfiguration configuration;

        public KillRateServiceTest()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();

            dynamoDBAccessor = new DynamoDBAccessor(configuration);
            authService = new MockAuthService();
            killRateService = new KillRateService(dynamoDBAccessor, authService);
        }

        [TestMethod]
        public async Task AddKillRateTest()
        {
            var userId = Guid.NewGuid();
            MockAuthService.UserId = userId;

            var request = new KillRateRequest()
            {
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 2,
                Death = 3,
                Special = 4,
                Date = "2025-01-13 12:00",
                MatchTime = 180,
            };

            var response = await killRateService.AddKillRate(request);

            Assert.IsNotNull(response.Id);
            Debug.WriteLine(response.Id);

            var killRates = await dynamoDBAccessor.GetKillRates();

            Assert.IsTrue(killRates.Exists(killRate => killRate.UserId == userId.ToString() && killRate.Id == response.Id));
        }

        [TestMethod]
        public async Task UpdateKillRateTest()
        {
            var userId = Guid.NewGuid();
            MockAuthService.UserId = userId;

            var request = new KillRateRequest()
            {
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 2,
                Death = 3,
                Special = 4,
                Date = "2025-01-13 12:00",
                MatchTime = 180,
            };

            var response = await killRateService.AddKillRate(request);

            request.Kill = 20;

            await killRateService.UpdateKillRate(response.Id, request);

            var killRates = await dynamoDBAccessor.GetKillRates();

            Assert.IsTrue(killRates.Exists(killRate => killRate.UserId == userId.ToString() && killRate.Id == response.Id && killRate.Kill == 20));
        }

        [TestMethod]
        public async Task DeleteKillRateTest()
        {
            var userId = Guid.NewGuid();
            MockAuthService.UserId = userId;

            var request = new KillRateRequest()
            {
                Battle = Splatoon3Enums.BattleType.Regular.ToString(),
                Rule = Splatoon3Enums.RuleType.TurfWar.ToString(),
                Weapon = Splatoon3Enums.Weapon.Splattershot.ToString(),
                Result = Splatoon3Enums.ResultType.Win.ToString(),
                Kill = 10,
                Assist = 2,
                Death = 3,
                Special = 4,
                Date = "2025-01-13 12:00",
                MatchTime = 180,
            };

            var response = await killRateService.AddKillRate(request);

            await killRateService.DeleteKillRate(response.Id);

            var killRates = await dynamoDBAccessor.GetKillRates();

            Assert.IsFalse(killRates.Exists(killRate => killRate.UserId == userId.ToString() && killRate.Id == response.Id));
        }
    }
}
