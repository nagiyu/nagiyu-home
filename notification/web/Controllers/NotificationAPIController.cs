using Microsoft.AspNetCore.Mvc;
using Nagiyu.Common.Auth.Service.Interfaces;
using Nagiyu.Common.Auth.Service.Models;
using System.Threading.Tasks;

namespace Nagiyu.Notification.Web.Controllers
{
    /// <summary>
    /// Notification Controller (API)
    /// </summary>
    public class NotificationAPIController : Controller
    {
        /// <summary>
        /// AuthService
        /// </summary>
        private readonly IAuthService authService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="authService">AuthService</param>
        public NotificationAPIController(IAuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Subscription ID 登録
        /// </summary>
        /// <param name="id">Subscription ID</param>
        [HttpPost]
        [Route("api/notification/{id}")]
        public async Task RegisterSubscriptionId([FromRoute] string id)
        {
            var user = await authService.GetUser<UserAuthBase>();

            if (user == null)
            {
                return;
            }

            user.OneSignalSubscriptionId = id;

            await authService.UpdateUser(user);
        }
    }
}
