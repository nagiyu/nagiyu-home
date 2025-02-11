using System;
using Nagiyu.Common.Service.Consts;

namespace Nagiyu.Common.Service.Models.Auth
{
    /// <summary>
    /// ユーザー情報
    /// </summary>
    public class UserBase
    {
        /// <summary>
        /// ユーザー ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Google ユーザー ID
        /// </summary>
        public string GoogleUserId { get; set; }

        /// <summary>
        /// システムロール
        /// </summary>
        public SystemRoleEnums.SystemRole SystemRole { get; set; } = SystemRoleEnums.SystemRole.None;
    }
}
