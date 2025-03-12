using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;

namespace Nagiyu.Common.Service.Utilities
{
    public static class DynamoDBUtil
    {
        public static string GetStringValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.S, throwIfNotFound);
        }

        public static List<string> GetStringListValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.SS, throwIfNotFound);
        }

        public static int GetIntValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => int.Parse(v.N), throwIfNotFound);
        }

        public static List<int> GetIntListValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.NS.ConvertAll(int.Parse), throwIfNotFound);
        }

        public static long GetLongValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => long.Parse(v.N), throwIfNotFound);
        }

        public static List<long> GetLongListValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.NS.ConvertAll(long.Parse), throwIfNotFound);
        }

        public static double GetDoubleValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => double.Parse(v.N), throwIfNotFound);
        }

        public static List<double> GetDoubleListValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.NS.ConvertAll(double.Parse), throwIfNotFound);
        }

        public static bool GetBoolValue(Dictionary<string, AttributeValue> keyValuePairs, string key, bool throwIfNotFound = true)
        {
            return GetValueOrThrow(keyValuePairs, key, v => v.BOOL, throwIfNotFound);
        }

        private static T GetValueOrThrow<T>(Dictionary<string, AttributeValue> keyValuePairs, string key, Func<AttributeValue, T> selector, bool throwIfNotFound)
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
                if (throwIfNotFound)
                {
                    throw new KeyNotFoundException(key);
                }
                else
                {
                    return default;
                }
            }
        }
    }
}
