using System.Collections.Generic;

namespace Nagiyu.Common.Service.Models.Notification.Requests
{
    /// <summary>
    /// Notification Request (Segments)
    /// </summary>
    internal class NotificationSegmentsRequest
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
    }
}
