using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Service.Services;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nagiyu.Common.Service.Tests.Services
{
    [TestClass]
    public class NotificationServiceTest
    {
        private readonly NotificationService notificationService;

        public NotificationServiceTest()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var httpClient = new HttpClient();

            notificationService = new NotificationService(configuration, httpClient);
        }

        [TestMethod]
        public async Task PushTotalSubscriptionsTest()
        {
            var message = "Test Message";
            await notificationService.PushTotalSubscriptions(message);
        }
    }
}
