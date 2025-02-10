using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        [TestMethod]
        public async Task AddRecordTest()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            await service.AddRecord(record);

            var records = await service.GetAllRecords<TestRecord>();

            Assert.IsTrue(records.Exists(r => r.Id == record.Id));
        }

        [TestMethod]
        public async Task UpdateRecordTest()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            await service.AddRecord(record);

            await Task.Delay(1000);

            record.Column1 = "UpdatedColumn1";

            await service.UpdateRecord(record);

            var records = await service.GetAllRecords<TestRecord>();

            Assert.IsTrue(records.Exists(r => r.Column1 == record.Column1));
        }

        [TestMethod]
        public async Task DeleteRecordTest()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            await service.AddRecord(record);

            await service.DeleteRecord(record);

            var records = await service.GetAllRecords<TestRecord>();

            Assert.IsFalse(records.Exists(r => r.Id == record.Id));
        }
    }

    internal class TestRecord : RecordBase
    {
        [DynamoDBProperty]
        public string Column1 { get; set; }

        [DynamoDBProperty]
        public string IndexId { get; set; }

        [DynamoDBProperty]
        public string IndexColumn1 { get; set; }

        public TestRecord() : base()
        {
        }

        public TestRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(Column1), out var column1))
            {
                Column1 = column1.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(Column1));
            }

            if (keyValuePairs.TryGetValue(nameof(IndexId), out var indexId))
            {
                IndexId = indexId.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(IndexId));
            }

            if (keyValuePairs.TryGetValue(nameof(IndexColumn1), out var indexColumn1))
            {
                IndexColumn1 = indexColumn1.S;
            }
            else
            {
                throw new KeyNotFoundException(nameof(IndexColumn1));
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
