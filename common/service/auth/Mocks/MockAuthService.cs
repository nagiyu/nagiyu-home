using Nagiyu.Common.Auth.Service.Consts;
using Nagiyu.Common.Auth.Service.Interfaces;
using Nagiyu.Common.Auth.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nagiyu.Common.Auth.Service.Mocks
{
    public class MockAuthService : IAuthService
    {
        public static Guid UserId = Guid.NewGuid();

        public static string SystemSubscriptionId = string.Empty;

        public Task<List<T>> GetAllUsers<T>() where T : UserAuthBase
        {
            return Task.FromResult(new List<T>()
            {
                CreateSystemUser() as T
            });
        }

        public Task<T> GetUser<T>() where T : UserAuthBase
        {
            return Task.FromResult(CreateSystemUser() as T);
        }

        public Task UpdateUser<T>(T user) where T : UserAuthBase
        {
            throw new NotImplementedException();
        }

        private UserAuthBase CreateSystemUser()
        {
            return new UserAuthBase()
            {
                UserId = UserId,
                UserName = "Test",
                GoogleUserId = "GoogleUserID",
                SystemRole = SystemRoleConsts.ADMIN,
                OneSignalSubscriptionId = SystemSubscriptionId
            };
        }
    }
}
