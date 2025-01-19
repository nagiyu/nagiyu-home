using Nagiyu.Common.Auth.Service.Models;
using System.Threading.Tasks;

namespace Nagiyu.Common.Auth.Service.Interfaces
{
    /// <summary>
    /// 認証サービスのインターフェース
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// ユーザー情報を取得する
        /// </summary>
        /// <returns>ユーザー情報</returns>
        /// <remarks>未認証: null, 認証済: ユーザー情報</remarks>
        public Task<T> GetUser<T>() where T : UserAuthBase;

        /// <summary>
        /// ユーザー情報を更新する
        /// </summary>
        /// <param name="user">ユーザー情報</param>
        public Task UpdateUser<T>(T user) where T : UserAuthBase;
    }
}
