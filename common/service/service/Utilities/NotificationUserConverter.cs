using Nagiyu.Common.Service.Models.DB.Notification;
using Nagiyu.Common.Service.Models.Notification;

namespace Nagiyu.Common.Service.Utilities
{
    /// <summary>
    /// 通知ユーザー情報のモデル変換クラス
    /// </summary>
    internal static class NotificationUserConverter
    {
        /// <summary>
        /// 通知ユーザー情報のレコードを通知ユーザー情報に変換する
        /// </summary>
        /// <param name="record">通知ユーザー情報のレコード</param>
        /// <returns>通知ユーザー情報</returns>
        public static NotificationUser ConvertToNotificationUser(NotificationUserRecord record)
        {
            var userBase = UserModelConverter.ConvertToUserBase(record);

            return new NotificationUser
            {
                UserId = userBase.UserId,
                UserName = userBase.UserName,
                GoogleUserId = userBase.GoogleUserId,
                SystemRole = userBase.SystemRole,
                OneSignalSubscriptionIdList = record.OneSignalSubscriptionIdList
            };
        }

        /// <summary>
        /// 通知ユーザー情報を通知ユーザー情報のレコードに変換する
        /// </summary>
        /// <param name="user">通知ユーザー情報</param>
        /// <returns>通知ユーザー情報のレコード</returns>
        public static NotificationUserRecord ConvertToNotificationUserRecord(NotificationUser user)
        {
            var userRecord = UserModelConverter.ConvertToUserRecord(user);

            return new NotificationUserRecord
            {
                UserId = userRecord.UserId,
                UserName = userRecord.UserName,
                GoogleUserId = userRecord.GoogleUserId,
                SystemRole = userRecord.SystemRole,
                OneSignalSubscriptionIdList = user.OneSignalSubscriptionIdList
            };
        }
    }
}
