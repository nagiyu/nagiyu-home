using Microsoft.AspNetCore.Mvc;
using Nagiyu.Common.Auth.Service.Interfaces;
using Nagiyu.Common.Auth.Service.Models;
using Nagiyu.Common.Service.Models.API.Requests;
using Nagiyu.Common.Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nagiyu.Common.Web.Controllers
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
        /// NotificationService
        /// </summary>
        private readonly NotificationService notification;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="authService">AuthService</param>
        public NotificationAPIController(IAuthService authService, NotificationService notification)
        {
            this.authService = authService;
            this.notification = notification;
        }

        /// <summary>
        /// Subscription ID 登録
        /// </summary>
        /// <param name="id">Subscription ID</param>
        [HttpPost]
        [Route("api/notification/{id}")]
        [Obsolete]
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

        /// <summary>
        /// Subscription ID によるプッシュ通知
        /// </summary>
        /// <param name="request">リクエスト</param>
        [HttpPost]
        [Route("api/notification/push-by-subscription-ids")]
        public async Task PushBySubscriptionIds([FromBody] PushBySubscriptionIdsRequest request)
        {
            await notification.PushSubscriptions(request.Message, request.SubscriptionIds);
        }

        /// <summary>
        /// ログインユーザーにプッシュ通知
        /// </summary>
        /// <param name="request">リクエスト</param>
        [HttpPost]
        [Route("api/notification/push-by-login-user")]
        public async Task PushNotifyByLoginUser([FromBody] PushNotifyByLoginUserRequest request)
        {
            var user = await authService.GetUser<UserAuthBase>();

            if (user == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(user.OneSignalSubscriptionId))
            {
                return;
            }

            await notification.PushSubscriptions(request.Message, new List<string> { user.OneSignalSubscriptionId });
        }
    }
}
