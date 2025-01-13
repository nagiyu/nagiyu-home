using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Nagiyu.Splatoon3Tracker.Service.Consts;
using System;
using System.Collections.Generic;

namespace Nagiyu.Splatoon3Tracker.Service.Models.DB
{
    public class ObjectBase
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        private string _recordType;

        [DynamoDBProperty]
        public string RecordType
        {
            get => _recordType;
            set
            {
                if (Enum.TryParse<Splatoon3Enums.RecordType>(value, out var recordType))
                {
                    _recordType = recordType.ToString();
                }
                else
                {
                    _recordType = string.Empty;
                }
            }
        }

        public ObjectBase()
        {
        }

        public ObjectBase(Dictionary<string, AttributeValue> keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(Id), out var idValue) && Guid.TryParse(idValue.S, out var id))
            {
                Id = id;
            }
            else
            {
                Id = Guid.Empty;
            }

            if (keyValuePairs.TryGetValue(nameof(RecordType), out var recordType))
            {
                RecordType = recordType.S;
            }
            else
            {
                RecordType = string.Empty;
            }
        }
    }
}
