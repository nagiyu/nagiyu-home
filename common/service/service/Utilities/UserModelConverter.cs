using System;
using Nagiyu.Common.Service.Consts;
using Nagiyu.Common.Service.Models.Auth;
using Nagiyu.Common.Service.Models.DB.Auth;

namespace Nagiyu.Common.Service.Utilities
{
    /// <summary>
    /// ユーザー情報のモデル変換クラス
    /// </summary>
    internal static class UserModelConverter
    {
        /// <summary>
        /// ユーザー情報のレコードをユーザー情報の基底クラスに変換する
        /// </summary>
        /// <param name="record">ユーザー情報のレコード</param>
        /// <returns>ユーザー情報の基底クラス</returns>
        public static UserBase ConvertToUserBase(UserRecord record)
        {
            if (!Guid.TryParse(record.UserId, out var userId))
            {
                throw new ArgumentException($"{nameof(UserRecord.UserId)} is invalid.");
            }

            if (!Enum.TryParse(record.SystemRole, out SystemRoleEnums.SystemRole systemRole))
            {
                throw new ArgumentException($"{nameof(UserRecord.SystemRole)} is invalid.");
            }

            return new UserBase
            {
                UserId = userId,
                UserName = record.UserName,
                GoogleUserId = record.GoogleUserId,
                SystemRole = systemRole
            };
        }

        /// <summary>
        /// ユーザー情報の基底クラスをユーザー情報のレコードに変換する
        /// </summary>
        /// <param name="user">ユーザー情報の基底クラス</param>
        /// <returns>ユーザー情報のレコード</returns>
        public static UserRecord ConvertToUserRecord(UserBase user)
        {
            return new UserRecord
            {
                UserId = user.UserId.ToString(),
                UserName = user.UserName,
                GoogleUserId = user.GoogleUserId,
                SystemRole = user.SystemRole.ToString()
            };
        }
    }
}
