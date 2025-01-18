using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Nagiyu.Splatoon3Tracker.Service.Exceptions;
using System.Collections.Generic;

namespace Nagiyu.Splatoon3Tracker.Service.Models.DB
{
    /// <summary>
    /// Splatoon3Tracker の基底クラス
    /// </summary>
    public abstract class Splatoon3TrackerRecordBase
    {
        /// <summary>
        /// ID
        /// </summary>
        [DynamoDBHashKey]
        public string Id { get; set; }

        /// <summary>
        /// レコードのタイプ
        /// </summary>
        [DynamoDBProperty]
        public string RecordType { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Splatoon3TrackerRecordBase()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyValuePairs">DynamoDB の AttributeValue</param>
        public Splatoon3TrackerRecordBase(Dictionary<string, AttributeValue> keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(Id), out var idValue))
            {
                Id = idValue.S;
            }
            else
            {
                throw new ParameterException("Id is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(RecordType), out var recordType))
            {
                RecordType = recordType.S;
            }
            else
            {
                throw new ParameterException("RecordType is not found.");
            }
        }
    }
}
