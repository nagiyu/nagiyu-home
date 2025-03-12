using System.Collections.Generic;
using System.IO;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using Nagiyu.Common.Service.Models.DB;
using Nagiyu.Common.Service.Services;
using Nagiyu.Common.Service.Utilities;

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

        public TestRecord() : base()
        {
        }

        public TestRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            Column1 = DynamoDBUtil.GetStringValue(keyValuePairs, nameof(Column1));
            StringColumns = DynamoDBUtil.GetStringListValue(keyValuePairs, nameof(StringColumns));
            IntColumn = DynamoDBUtil.GetIntValue(keyValuePairs, nameof(IntColumn));
            IntColumns = DynamoDBUtil.GetIntListValue(keyValuePairs, nameof(IntColumns));
            LongColumn = DynamoDBUtil.GetLongValue(keyValuePairs, nameof(LongColumn));
            LongColumns = DynamoDBUtil.GetLongListValue(keyValuePairs, nameof(LongColumns));
            DoubleColumn = DynamoDBUtil.GetDoubleValue(keyValuePairs, nameof(DoubleColumn));
            DoubleColumns = DynamoDBUtil.GetDoubleListValue(keyValuePairs, nameof(DoubleColumns));
            BoolColumn = DynamoDBUtil.GetBoolValue(keyValuePairs, nameof(BoolColumn));
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
            Column2 = DynamoDBUtil.GetStringValue(keyValuePairs, nameof(Column2));
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
