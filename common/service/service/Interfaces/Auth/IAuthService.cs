using System;
using System.Threading.Tasks;
using Nagiyu.Common.Service.Models.Auth;

namespace Nagiyu.Common.Service.Interfaces.Auth
{
    /// <summary>
    /// 認証サービスのインターフェース
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// ユーザーID からユーザー情報を取得する
        /// </summary>
        /// <param name="userId">ユーザー ID</param>
        /// <returns>ユーザー情報</returns>
        public Task<UserBase> GetUserByUserId(Guid userId);

        /// <summary>
        /// GoogleユーザーID からユーザー情報を取得する
        /// </summary>
        /// <param name="googleUserId">Google ユーザー ID</param>
        /// <returns>ユーザー情報</returns>
        public Task<UserBase> GetUserByGoogleUserId(string googleUserId);

        /// <summary>
        /// ユーザー情報を追加する
        /// </summary>
        /// <param name="user">ユーザー情報</param>
        /// <returns>ユーザー ID</returns>
        public Task<Guid> AddUser(UserBase user);

        /// <summary>
        /// ユーザー情報を更新する
        /// </summary>
        /// <param name="user">ユーザー情報</param>
        public Task UpdateUser(UserBase user);

        /// <summary>
        /// ユーザー情報を削除する
        /// </summary>
        /// <param name="userId">ユーザー ID</param>
        public Task DeleteUser(Guid userId);
    }
}
