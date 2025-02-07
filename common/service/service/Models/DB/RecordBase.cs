using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace Nagiyu.Common.Service.Models.DB
{
    /// <summary>
    /// DynamoDB レコードの基底クラス
    /// </summary>
    public abstract class RecordBase
    {
        /// <summary>
        /// ID
        /// </summary>
        [DynamoDBHashKey]
        public string Id { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [DynamoDBProperty]
        public string CreatedAt { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [DynamoDBProperty]
        public string UpdatedAt { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RecordBase()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyValuePairs">キーと値のペア</param>
        public RecordBase(Dictionary<string, AttributeValue> keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(Id), out var id))
            {
                Id = id.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(Id));
            }

            if (keyValuePairs.TryGetValue(nameof(CreatedAt), out var createdAt))
            {
                CreatedAt = createdAt.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(CreatedAt));
            }

            if (keyValuePairs.TryGetValue(nameof(UpdatedAt), out var updatedAt))
            {
                UpdatedAt = updatedAt.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(UpdatedAt));
            }
        }
    }
}
