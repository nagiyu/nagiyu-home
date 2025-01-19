using System.Collections.Generic;

namespace Nagiyu.Common.Service.Models.Notification.Requests
{
    /// <summary>
    /// Notification Request (Subscriptions)
    /// </summary>
    internal class NotificationSubscriptionRequest
    {
        /// <summary>
        /// Application ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Contents
        /// </summary>
        public NotificationContents Contents { get; set; }

        /// <summary>
        /// Subscription IDs
        /// </summary>
        public List<string> IncludeSubscriptionIds { get; set; }
    }
}