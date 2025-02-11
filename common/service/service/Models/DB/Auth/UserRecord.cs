using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace Nagiyu.Common.Service.Models.DB.Auth
{
    /// <summary>
    /// ユーザー情報のレコード
    /// </summary>
    internal class UserRecord : RecordBase
    {
        /// <summary>
        /// ユーザー ID
        /// </summary>
        [DynamoDBProperty]
        public string UserId { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [DynamoDBProperty]
        public string UserName { get; set; }

        /// <summary>
        /// Google ユーザー ID
        /// </summary>
        [DynamoDBProperty]
        public string GoogleUserId { get; set; }

        /// <summary>
        /// システムロール
        /// </summary>
        [DynamoDBProperty]
        public string SystemRole { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserRecord() : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyValuePairs">キーと値のペア</param>
        public UserRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(UserId), out var userId) && userId.S != null)
            {
                UserId = userId.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(UserId));
            }

            if (keyValuePairs.TryGetValue(nameof(UserName), out var userName) && userName.S != null)
            {
                UserName = userName.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(UserName));
            }

            if (keyValuePairs.TryGetValue(nameof(GoogleUserId), out var googleUserId) && googleUserId.S != null)
            {
                GoogleUserId = googleUserId.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(GoogleUserId));
            }

            if (keyValuePairs.TryGetValue(nameof(SystemRole), out var systemRole) && systemRole.S != null)
            {
                SystemRole = systemRole.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(SystemRole));
            }
        }
    }
}
