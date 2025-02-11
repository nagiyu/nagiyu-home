using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nagiyu.Common.Service.Interfaces.Auth;
using Nagiyu.Common.Service.Models.Auth;
using Nagiyu.Common.Service.Models.DB.Auth;
using Nagiyu.Common.Service.Utilities;

namespace Nagiyu.Common.Service.Services.Auth
{
    /// <summary>
    /// 認証サービス
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// ユーザー情報の DynamoDB サービス
        /// </summary>
        private readonly UserDynamoDBService dbService;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="userDynamoDBService">ユーザー情報の DynamoDB サービス</param>
        public AuthService(UserDynamoDBService userDynamoDBService)
        {
            dbService = userDynamoDBService;
        }

        /// <summary>
        /// ユーザー情報を取得する
        /// </summary>
        /// <param name="userId">ID</param>
        /// <returns>ユーザー情報</returns>
        public async Task<UserBase> GetUser(Guid id)
        {
            var record = await dbService.GetRecord<UserRecord>(id);

            ValidateUser.ValidateUserRecord(record);

            return UserModelConverter.ConvertToUserBase(record);
        }

        /// <summary>
        /// ユーザーID からユーザー情報を取得する
        /// </summary>
        /// <param name="userId">ユーザー ID</param>
        /// <returns>ユーザー情報</returns>
        public async Task<UserBase> GetUserByUserId(Guid userId)
        {
            var record = await GetUserRecordByUserId(userId);
            return UserModelConverter.ConvertToUserBase(record);
        }

        /// <summary>
        /// GoogleユーザーID からユーザー情報を取得する
        /// </summary>
        /// <param name="googleUserId">Google ユーザー ID</param>
        /// <returns>ユーザー情報</returns>
        public async Task<UserBase> GetUserByGoogleUserId(string googleUserId)
        {
            var expressions = new Dictionary<string, string>
            {
                { nameof(UserRecord.GoogleUserId), googleUserId }
            };
            var records = await dbService.GetRecords<UserRecord>(expressions: expressions);
            var record = records.Item1.FirstOrDefault();

            ValidateUser.ValidateUserRecord(record);

            return UserModelConverter.ConvertToUserBase(record);
        }

        /// <summary>
        /// ユーザー情報を追加する
        /// </summary>
        /// <param name="user">ユーザー情報</param>
        /// <returns>ユーザー ID</returns>
        public async Task<Guid> AddUser(UserBase user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var userId = Guid.NewGuid();
            user.UserId = userId;

            ValidateUser.ValidateUserBase(user);

            var record = UserModelConverter.ConvertToUserRecord(user);
            await dbService.AddRecord(record);

            return userId;
        }

        /// <summary>
        /// ユーザー情報を更新する
        /// </summary>
        /// <param name="user">ユーザー情報</param>
        public async Task UpdateUser(UserBase user)
        {
            ValidateUser.ValidateUserBase(user);

            var dbRecord = await GetUserRecordByUserId(user.UserId);
            if (dbRecord == null || !Guid.TryParse(dbRecord.Id, out var id))
            {
                throw new ArgumentException($"{nameof(UserRecord)} not found.");
            }

            var record = UserModelConverter.ConvertToUserRecord(user);

            await dbService.UpdateRecord(id, record);
        }

        /// <summary>
        /// ユーザー情報を削除する
        /// </summary>
        /// <param name="userId">ユーザー ID</param>
        public async Task DeleteUser(Guid userId)
        {
            var record = await GetUserRecordByUserId(userId);

            ValidateUser.ValidateUserRecord(record);

            await dbService.DeleteRecord(record);
        }

        /// <summary>
        /// GoogleユーザーID からユーザー情報のレコードを取得する
        /// </summary>
        /// <param name="userId">ユーザー ID</param>
        /// <returns>ユーザー情報のレコード</returns>
        private async Task<UserRecord> GetUserRecordByUserId(Guid userId)
        {
            var expressions = new Dictionary<string, string>
            {
                { nameof(UserRecord.UserId), userId.ToString() }
            };
            var records = await dbService.GetRecords<UserRecord>(expressions: expressions);
            var record = records.Item1.FirstOrDefault();

            ValidateUser.ValidateUserRecord(record);

            return record;
        }
    }
}
