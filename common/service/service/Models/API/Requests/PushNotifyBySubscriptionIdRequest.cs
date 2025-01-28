using System.Collections.Generic;

namespace Nagiyu.Common.Service.Models.API.Requests
{
    public class PushBySubscriptionIdsRequest
    {
        /// <summary>
        /// SubscriptionIds
        /// </summary>
        public List<string> SubscriptionIds { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message { get; set; }
    }
}
