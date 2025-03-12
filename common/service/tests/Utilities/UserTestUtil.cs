using System;

namespace Nagiyu.Common.Service.Tests.Utilities
{
    /// <summary>
    /// ユーザー情報のテストユーティリティ
    /// </summary>
    internal static class UserTestUtil
    {
        /// <summary>
        /// テスト用のユーザー ID を生成する
        /// </summary>
        /// <returns>ユーザー ID</returns>
        public static string GenerateGoogleUserId()
        {
            var random = new Random();
            var buffer = new byte[8];
            random.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0).ToString();
        }
    }
}
