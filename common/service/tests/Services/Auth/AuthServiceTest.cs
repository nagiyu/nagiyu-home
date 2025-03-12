using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Service.Consts;
using Nagiyu.Common.Service.Interfaces.Auth;
using Nagiyu.Common.Service.Models.Auth;
using Nagiyu.Common.Service.Services.Auth;
using Nagiyu.Common.Service.Tests.Utilities;

namespace Nagiyu.Common.Service.Tests.Services.Auth
{
    [TestClass]
    public class AuthServiceTest
    {
        private readonly IAuthService authService;

        public AuthServiceTest()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            var dbService = new UserDynamoDBService(configuration);

            authService = new AuthService(dbService);
        }

        [TestMethod]
        public async Task GetUserByUserId()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            var userId = await authService.AddUser(user);

            var result = await authService.GetUserByUserId(userId);

            AssertUser(user, result);
        }

        [TestMethod]
        public async Task GetUserByUserId_NotFoundUserId()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await authService.GetUserByUserId(Guid.Empty));
        }

        [TestMethod]
        public async Task GetUserByGoogleUserId()
        {
            var googleUserId = UserTestUtil.GenerateGoogleUserId();

            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = googleUserId,
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            await authService.AddUser(user);

            var result = await authService.GetUserByGoogleUserId(googleUserId);

            AssertUser(user, result);
        }

        [TestMethod]
        public async Task GetUserByGoogleUserId_NotFoundGoogleUserId()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await authService.GetUserByGoogleUserId(UserTestUtil.GenerateGoogleUserId()));
        }

        [TestMethod]
        public async Task GetUsersByRole()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.Admin
            };
            await authService.AddUser(user);

            var result = await authService.GetUsersByRole(SystemRoleEnums.SystemRole.Admin);

            Assert.IsTrue(result.Exists(u => u.UserId == user.UserId));
        }

        [TestMethod]
        public async Task AddUser()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            var userId = await authService.AddUser(user);

            var result = await authService.GetUserByUserId(userId);

            AssertUser(user, result);
        }

        [TestMethod]
        public async Task AddUser_NullUser()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await authService.AddUser(null));
        }

        [TestMethod]
        public async Task AddUser_EmptyUserName()
        {
            var user = new UserBase
            {
                UserName = string.Empty,
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            var userId = await authService.AddUser(user);

            var result = await authService.GetUserByUserId(userId);

            AssertUser(user, result);
        }

        [TestMethod]
        public async Task AddUser_NullUserName()
        {
            var user = new UserBase
            {
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await authService.AddUser(user));
        }

        [TestMethod]
        public async Task AddUser_NullGoogleUserId()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                SystemRole = SystemRoleEnums.SystemRole.None
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await authService.AddUser(user));
        }

        [TestMethod]
        public async Task AddUser_NullSystemRole()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId()
            };
            var userId = await authService.AddUser(user);

            var result = await authService.GetUserByUserId(userId);

            user.SystemRole = SystemRoleEnums.SystemRole.None;

            AssertUser(user, result);
        }

        [TestMethod]
        public async Task UpdateUser()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            var userId = await authService.AddUser(user);

            user.UserName = "UpdatedUserName";

            await authService.UpdateUser(user);

            var result = await authService.GetUserByUserId(userId);

            AssertUser(user, result);
        }

        [TestMethod]
        public async Task UpdateUser_NullUser()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await authService.UpdateUser(null));
        }

        [TestMethod]
        public async Task UpdateUser_EmptyToSetUserName()
        {
            var user = new UserBase
            {
                UserName = string.Empty,
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            var userId = await authService.AddUser(user);

            user.UserName = "UserName";

            await authService.UpdateUser(user);

            var result = await authService.GetUserByUserId(userId);

            AssertUser(user, result);
        }

        [TestMethod]
        public async Task UpdateUser_NullUserName()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            var userId = await authService.AddUser(user);

            user.UserName = null;

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await authService.UpdateUser(user));
        }

        [TestMethod]
        public async Task UpdateUser_NullGoogleUserId()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            var userId = await authService.AddUser(user);

            user.GoogleUserId = null;

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await authService.UpdateUser(user));
        }

        [TestMethod]
        public async Task DeleteUser()
        {
            var user = new UserBase
            {
                UserName = "UserName",
                GoogleUserId = UserTestUtil.GenerateGoogleUserId(),
                SystemRole = SystemRoleEnums.SystemRole.None
            };
            var userId = await authService.AddUser(user);

            await authService.DeleteUser(userId);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await authService.GetUserByUserId(userId));
        }

        private static void AssertUser(UserBase expected, UserBase actual)
        {
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.UserName, actual.UserName);
            Assert.AreEqual(expected.GoogleUserId, actual.GoogleUserId);
            Assert.AreEqual(expected.SystemRole, actual.SystemRole);
        }
    }
}
