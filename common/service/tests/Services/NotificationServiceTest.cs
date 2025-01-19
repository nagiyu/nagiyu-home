using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Auth.Service.Mocks;
using Nagiyu.Common.Service.Services;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nagiyu.Common.Service.Tests.Services
{
    [TestClass]
    public class NotificationServiceTest
    {
        private readonly IConfiguration configuration;

        private readonly NotificationService notificationService;

        public NotificationServiceTest()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();

            var httpClient = new HttpClient();

            var authService = new MockAuthService();

            notificationService = new NotificationService(configuration, httpClient, authService);
        }

        [TestMethod]
        public async Task PushNotifyOnlySystemRoleTest()
        {
            MockAuthService.SystemSubscriptionId = configuration["SystemSubscriptionId"];

            await notificationService.PushNotifyOnlySystemRole("System Message Test");
        }

        [TestMethod]
        public async Task PushNotifyAllTest()
        {
            await notificationService.PushNotifyAll("Test Message");
        }
    }
}
