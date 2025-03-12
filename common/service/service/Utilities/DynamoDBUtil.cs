using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace Nagiyu.Common.Service.Utilities
{
    public static class DynamoDBUtil
    {
        public static string GetStringValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.S);
        }

        public static List<string> GetStringListValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.SS);
        }

        public static int GetIntValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => int.Parse(v.N));
        }

        public static List<int> GetIntListValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.NS.ConvertAll(int.Parse));
        }

        public static long GetLongValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => long.Parse(v.N));
        }

        public static List<long> GetLongListValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.NS.ConvertAll(long.Parse));
        }

        public static double GetDoubleValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => double.Parse(v.N));
        }

        public static List<double> GetDoubleListValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.NS.ConvertAll(double.Parse));
        }

        public static bool GetBoolValue(Dictionary<string, AttributeValue> keyValuePairs, string key)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.BOOL);
        }

        private static T GetValueOrThrow<T>(Dictionary<string, AttributeValue> keyValuePairs, string key, Func<AttributeValue, T> selector)
        {
            if (keyValuePairs.TryGetValue(key, out var value))
            {
                try
                {
                    return selector(value);
                }
                catch (Exception)
                {
                    throw new InvalidCastException(key);
                }
            }
            else
            {
                throw new KeyNotFoundException(key);
            }
        }
    }
}
