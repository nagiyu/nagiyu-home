using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Nagiyu.Common.Service.Models.DB.Auth;

namespace Nagiyu.Common.Service.Models.DB.Notification
{
    /// <summary>
    /// 通知ユーザー情報のレコード
    /// </summary>
    public class NotificationUserRecord : UserRecord
    {
        /// <summary>
        /// OneSignal の Subscription ID リスト
        /// </summary>
        [DynamoDBProperty]
        public List<string> OneSignalSubscriptionIdList { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NotificationUserRecord() : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyValuePairs">キーと値のペア</param>
        public NotificationUserRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(OneSignalSubscriptionIdList), out var oneSignalSubscriptionIdList) && oneSignalSubscriptionIdList.IsSSSet)
            {
                OneSignalSubscriptionIdList = oneSignalSubscriptionIdList.SS;
            }
            else
            {
                OneSignalSubscriptionIdList = new List<string>();
            }
        }
    }
}
