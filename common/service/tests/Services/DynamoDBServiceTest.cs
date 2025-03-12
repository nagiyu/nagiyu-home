using System;
using System.Collections.Generic;
using System.IO;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Service.Models.DB;
using Nagiyu.Common.Service.Services;

namespace Nagiyu.Common.Service.Tests.Services
{
    [TestClass]
    public class DynamoDBServiceTest
    {
        private readonly TestDynamoDBService service;

        public DynamoDBServiceTest()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            service = new TestDynamoDBService(configuration);
        }
    }

    internal class TestRecord : RecordBase
    {
        [DynamoDBProperty]
        public string Column1 { get; set; }

        [DynamoDBProperty]
        public List<string> StringColumns { get; set; }

        [DynamoDBProperty]
        public int IntColumn { get; set; }

        [DynamoDBProperty]
        public List<int> IntColumns { get; set; }

        [DynamoDBProperty]
        public long LongColumn { get; set; }

        [DynamoDBProperty]
        public List<long> LongColumns { get; set; }

        [DynamoDBProperty]
        public double DoubleColumn { get; set; }

        [DynamoDBProperty]
        public List<double> DoubleColumns { get; set; }

        [DynamoDBProperty]
        public bool BoolColumn { get; set; }

        [DynamoDBProperty]
        public string IndexId { get; set; }

        [DynamoDBProperty]
        public string IndexColumn1 { get; set; }

        public TestRecord() : base()
        {
        }

        public TestRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(Column1), out var column1) && column1.S != null)
            {
                Column1 = column1.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(Column1));
            }

            if (keyValuePairs.TryGetValue(nameof(StringColumns), out var stringColumns) && stringColumns.IsSSSet)
            {
                StringColumns = stringColumns.SS;
            }
            else
            {
                throw new KeyNotFoundException(nameof(StringColumns));
            }

            if (keyValuePairs.TryGetValue(nameof(IntColumn), out var intColumn) && int.TryParse(intColumn.N, out var intColumnValue))
            {
                IntColumn = intColumnValue;
            }
            else
            {
                throw new KeyNotFoundException(nameof(IntColumn));
            }

            if (keyValuePairs.TryGetValue(nameof(IntColumns), out var intColumns) && intColumns.IsNSSet)
            {
                foreach (var value in intColumns.NS)
                {
                    if (int.TryParse(value, out var intValue))
                    {
                        IntColumns.Add(intValue);
                    }
                    else
                    {
                        throw new InvalidCastException(nameof(IntColumns));
                    }
                }
            }
            else
            {
                throw new KeyNotFoundException(nameof(IntColumns));
            }

            if (keyValuePairs.TryGetValue(nameof(LongColumn), out var longColumn) && long.TryParse(longColumn.N, out var longColumnValue))
            {
                LongColumn = longColumnValue;
            }
            else
            {
                throw new KeyNotFoundException(nameof(LongColumn));
            }

            if (keyValuePairs.TryGetValue(nameof(LongColumns), out var longColumns) && intColumns.IsNSSet)
            {
                foreach (var value in longColumns.NS)
                {
                    if (long.TryParse(value, out var intValue))
                    {
                        LongColumns.Add(intValue);
                    }
                    else
                    {
                        throw new InvalidCastException(nameof(LongColumns));
                    }
                }
            }
            else
            {
                throw new KeyNotFoundException(nameof(LongColumns));
            }

            if (keyValuePairs.TryGetValue(nameof(DoubleColumn), out var doubleColumn) && double.TryParse(doubleColumn.N, out var doubleColumnValue))
            {
                DoubleColumn = doubleColumnValue;
            }
            else
            {
                throw new KeyNotFoundException(nameof(DoubleColumn));
            }

            if (keyValuePairs.TryGetValue(nameof(DoubleColumns), out var doubleColumns) && doubleColumns.IsNSSet)
            {
                foreach (var value in doubleColumns.NS)
                {
                    if (double.TryParse(value, out var doubleValue))
                    {
                        DoubleColumns.Add(doubleValue);
                    }
                    else
                    {
                        throw new InvalidCastException(nameof(DoubleColumns));
                    }
                }
            }
            else
            {
                throw new KeyNotFoundException(nameof(DoubleColumns));
            }

            if (keyValuePairs.TryGetValue(nameof(BoolColumn), out var boolColumn) && boolColumn.IsBOOLSet)
            {
                BoolColumn = boolColumn.BOOL;
            }
            else
            {
                throw new KeyNotFoundException(nameof(BoolColumn));
            }

            if (keyValuePairs.TryGetValue(nameof(IndexId), out var indexId) && indexId.S != null)
            {
                IndexId = indexId.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(IndexId));
            }

            if (keyValuePairs.TryGetValue(nameof(IndexColumn1), out var indexColumn1) && indexColumn1.S != null)
            {
                IndexColumn1 = indexColumn1.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(IndexColumn1));
            }
        }
    }

    internal class ExtendedTestRecord : TestRecord
    {
        [DynamoDBProperty]
        public string Column2 { get; set; }

        public ExtendedTestRecord() : base()
        {
        }

        public ExtendedTestRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(Column2), out var column2) && column2.S != null)
            {
                Column2 = column2.S;
            }
            else
            {
                Column2 = null;
            }
        }
    }

    internal class TestDynamoDBService : DynamoDBServiceBase
    {
        public TestDynamoDBService(IConfiguration configuration) : base(configuration)
        {
            TableName = "Test";
        }
    }
}
