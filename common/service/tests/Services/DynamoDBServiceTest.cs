using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task GetRecordTest()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            var result = await service.GetRecord<TestRecord>(id);

            Assert.AreEqual(record.Id, result.Id);
        }

        [TestMethod]
        public async Task GetRecordsWithExpressions()
        {
            var record1 = new TestRecord
            {
                Column1 = "Column3",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn3"
            };
            var record2 = new TestRecord
            {
                Column1 = "Column3",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn4"
            };

            await service.AddRecord(record1);
            await service.AddRecord(record2);

            var expressions1 = new Dictionary<string, string>
            {
                { nameof(TestRecord.Column1), "Column3" }
            };
            var result1 = await service.GetRecords<TestRecord>(expressions: expressions1);

            foreach (var record in result1.Item1)
            {
                Assert.AreEqual("Column3", record.Column1);
            }

            var expressions2 = new Dictionary<string, string>
            {
                { nameof(TestRecord.Column1), "Column3" },
                { nameof(TestRecord.IndexColumn1), "IndexColumn3" }
            };
            var result2 = await service.GetRecords<TestRecord>(expressions: expressions2);

            foreach (var record in result2.Item1)
            {
                Assert.AreEqual("Column3", record.Column1);
                Assert.AreEqual("IndexColumn3", record.IndexColumn1);
            }
        }

        [TestMethod]
        public async Task GetRecordsWithLimit()
        {
            var record1 = new TestRecord
            {
                Column1 = "Column1",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };
            var record2 = new TestRecord
            {
                Column1 = "Column2",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn2"
            };

            await service.AddRecord(record1);
            await service.AddRecord(record2);

            var result1 = await service.GetRecords<TestRecord>(limit: 1);

            Assert.AreEqual(1, result1.Item1.Count);
            Assert.IsNotNull(result1.Item2);

            var result2 = await service.GetRecords<TestRecord>(limit: 1, startId: result1.Item2);

            Assert.AreNotEqual(result1.Item1.FirstOrDefault().Id, result2.Item1.FirstOrDefault().Id);

            var result3 = await service.GetRecords<TestRecord>(limit: 1000);

            Assert.IsNull(result3.Item2);
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

            var records = await service.GetRecords<TestRecord>();

            Assert.IsTrue(records.Item1.Exists(r => r.Id == record.Id));
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

            var records = await service.GetRecords<TestRecord>();

            Assert.IsTrue(records.Item1.Exists(r => r.Column1 == record.Column1));
        }

        [TestMethod]
        public async Task UpdateExtendedRecordTest()
        {
            var record = new ExtendedTestRecord
            {
                Column1 = "Column1",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            record.Column1 = "UpdatedColumn1";

            await Task.Delay(1000);

            await service.UpdateRecord(record);

            var result1 = await service.GetRecord<ExtendedTestRecord>(id);

            Assert.AreEqual(record.Column1, result1.Column1);
            Assert.AreEqual(record.Column2, result1.Column2);

            record.Column2 = "UpdatedColumn2";

            await Task.Delay(1000);

            await service.UpdateRecord(record);

            var result2 = await service.GetRecord<ExtendedTestRecord>(id);

            Assert.AreEqual(record.Column1, result2.Column1);
            Assert.AreEqual(record.Column2, result2.Column2);

            var newRecord = new ExtendedTestRecord
            {
                Id = id.ToString(),
                Column2 = "NewColumn2"
            };

            await Task.Delay(1000);

            await service.UpdateRecord(newRecord);

            var result3 = await service.GetRecord<ExtendedTestRecord>(id);

            Assert.AreEqual(record.Column1, result3.Column1);
            Assert.AreEqual(newRecord.Column2, result3.Column2);
        }

        [TestMethod]
        public async Task DeleteRecordByIdTest()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            await service.DeleteRecord(id);

            var records = await service.GetRecords<TestRecord>();

            Assert.IsFalse(records.Item1.Exists(r => r.Id == id.ToString()));
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

            var id = await service.AddRecord(record);

            await service.DeleteRecord(record);

            var records = await service.GetRecords<TestRecord>();

            Assert.IsFalse(records.Item1.Exists(r => r.Id == id.ToString()));
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

    internal class ExtendedTestRecord : TestRecord
    {
        [DynamoDBProperty]
        public string Column2 { get; set; }

        public ExtendedTestRecord() : base()
        {
        }

        public ExtendedTestRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(Column2), out var column2))
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
