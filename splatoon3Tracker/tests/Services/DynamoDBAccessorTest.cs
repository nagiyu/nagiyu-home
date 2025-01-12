using Microsoft.Extensions.Configuration;
using Nagiyu.Splatoon3Tracker.Service.Services;
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
        }
    }
}
