using System;
using Nagiyu.Common.Service.Models.Auth;
using Nagiyu.Common.Service.Models.DB.Auth;

namespace Nagiyu.Common.Service.Services.Auth
{
    /// <summary>
    /// ユーザー情報のバリデーション
    /// </summary>
    public static class ValidateUser
    {
        /// <summary>
        /// ユーザー情報のバリデーション
        /// </summary>
        /// <param name="user">ユーザー情報</param>
        public static void ValidateUserBase(UserBase user)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (user.UserId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(UserBase.UserId)} is empty.");
            }

            if (user.UserName == null)
            {
                throw new ArgumentException($"{nameof(UserBase.UserName)} is empty.");
            }

            if (string.IsNullOrWhiteSpace(user.GoogleUserId))
            {
                throw new ArgumentException($"{nameof(UserBase.GoogleUserId)} is empty.");
            }
        }

        /// <summary>
        /// ユーザー情報のレコードのバリデーション
        /// </summary>
        /// <param name="record">ユーザー情報のレコード</param>
        internal static void ValidateUserRecord(UserRecord record)
        {
            ArgumentNullException.ThrowIfNull(record);

            if (string.IsNullOrWhiteSpace(record.UserId))
            {
                throw new ArgumentException($"{nameof(UserRecord.UserId)} is empty.");
            }

            if (record.UserName == null)
            {
                throw new ArgumentException($"{nameof(UserRecord.UserName)} is empty.");
            }

            if (string.IsNullOrWhiteSpace(record.GoogleUserId))
            {
                throw new ArgumentException($"{nameof(UserRecord.GoogleUserId)} is empty.");
            }

            if (string.IsNullOrWhiteSpace(record.SystemRole))
            {
                throw new ArgumentException($"{nameof(UserRecord.SystemRole)} is empty.");
            }
        }
    }
}
