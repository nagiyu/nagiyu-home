using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace Nagiyu.Splatoon3Tracker.Service.Models.DB
{
    public class ObjectBase
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

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
        }
    }
}
