using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Nagiyu.Splatoon3Tracker.Service.Consts;
using System.Collections.Generic;

namespace Nagiyu.Splatoon3Tracker.Service.Models.DB
{
    public class KillRate : ObjectBase
    {
        [DynamoDBProperty]
        public Splatoon3Enums.RecordType RecordType { get; set; }

        [DynamoDBProperty]
        public string Battle { get; set; }

        [DynamoDBProperty]
        public string Rule { get; set; }

        [DynamoDBProperty]
        public string Weapon { get; set; }

        [DynamoDBProperty]
        public string Result { get; set; }

        [DynamoDBProperty]
        public string Kill { get; set; }

        [DynamoDBProperty]
        public string Assist { get; set; }

        [DynamoDBProperty]
        public string Death { get; set; }

        [DynamoDBProperty]
        public string Special { get; set; }

        [DynamoDBProperty]
        public string Date { get; set; }

        [DynamoDBProperty]
        public string MatchTime { get; set; }

        public KillRate() : base()
        {
        }

        public KillRate(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(RecordType), out var type) && Splatoon3Enums.RecordType.TryParse<Splatoon3Enums.RecordType>(type.S, out var recordType))
            {
                RecordType = recordType;
            }
            else
            {
                RecordType = Splatoon3Enums.RecordType.None;
            }
        }
    }
}
