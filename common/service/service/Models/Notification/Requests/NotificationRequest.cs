using System.Collections.Generic;

namespace Nagiyu.Common.Service.Models.Notification.Requests
{
    /// <summary>
    /// Notification Request
    /// </summary>
    internal class NotificationRequest
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
        /// Segments
        /// </summary>
        public List<string> IncludedSegments { get; set; }

        /// <summary>
        /// Subscription IDs
        /// </summary>
        public List<string> IncludeSubscriptionIds { get; set; }
    }

    /// <summary>
    /// Notification Contents
    /// </summary>
    internal class NotificationContents
    {
        /// <summary>
        /// English
        /// </summary>
        public string En { get; set; }
    }
}