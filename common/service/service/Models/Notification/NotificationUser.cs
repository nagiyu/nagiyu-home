using System.Collections.Generic;
using Nagiyu.Common.Service.Models.Auth;

namespace Nagiyu.Common.Service.Models.Notification
{
    /// <summary>
    /// 通知ユーザー情報
    /// </summary>
    public class NotificationUser : UserBase
    {
        /// <summary>
        /// OneSignal の Subscription ID リスト
        /// </summary>
        public List<string> OneSignalSubscriptionIdList { get; set; }
    }
}
