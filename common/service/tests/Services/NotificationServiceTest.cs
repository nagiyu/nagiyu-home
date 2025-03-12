using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Service.Consts;
using Nagiyu.Common.Service.Interfaces.Auth;
using Nagiyu.Common.Service.Models.Auth;
using Nagiyu.Common.Service.Services;
using Nagiyu.Common.Service.Services.Auth;
using Nagiyu.Common.Service.Tests.Utilities;

namespace Nagiyu.Common.Service.Tests.Services
{
    [TestClass]
    public class NotificationServiceTest
    {
        private readonly IAuthService authService;

        private readonly NotificationService notificationService;

        public NotificationServiceTest()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var httpClient = new HttpClient();
            var dbService = new UserDynamoDBService(configuration);

            authService = new AuthService(dbService);
            notificationService = new NotificationService(configuration, httpClient, authService, dbService);
        }

        [TestMethod]
        public async Task PushNotifyAllTest()
        {
            await notificationService.PushNotifyAll("Test Message");
        }

        [TestMethod]
        public async Task AddSubscriptionIdToUser_DuplicateSubscriptionId_ShouldNotAdd()
        {
            // Arrange
            var userId = await authService.AddUser(new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            });
            var subscriptionId = "test-subscription-id";

            // Act
            await notificationService.AddSubscriptionIdToUser(userId, subscriptionId);

            // Assert
            var subscriptionIds = await notificationService.GetSubscriptionIdsByUserId(userId);
            Assert.AreEqual(1, subscriptionIds.Count);

            // Act
            await notificationService.AddSubscriptionIdToUser(userId, subscriptionId);

            // Assert
            subscriptionIds = await notificationService.GetSubscriptionIdsByUserId(userId);
            Assert.AreEqual(1, subscriptionIds.Count);
        }

        [TestMethod]
        public async Task RemoveSubscriptionIdFromUser_NonExistentSubscriptionId_ShouldNotThrow()
        {
            // Arrange
            var userId = await authService.AddUser(new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            });
            var subscriptionId = "test-subscription-id";

            // Act
            await notificationService.AddSubscriptionIdToUser(userId, subscriptionId);

            // Assert
            var subscriptionIds = await notificationService.GetSubscriptionIdsByUserId(userId);
            Assert.AreEqual(1, subscriptionIds.Count);

            // Act
            await notificationService.RemoveSubscriptionIdFromUser(userId, subscriptionId);

            // Assert
            subscriptionIds = await notificationService.GetSubscriptionIdsByUserId(userId);
            Assert.AreEqual(0, subscriptionIds.Count);

            // Act
            await notificationService.RemoveSubscriptionIdFromUser(userId, subscriptionId);

            // Assert
            subscriptionIds = await notificationService.GetSubscriptionIdsByUserId(userId);
            Assert.AreEqual(0, subscriptionIds.Count);
        }
    }
}
