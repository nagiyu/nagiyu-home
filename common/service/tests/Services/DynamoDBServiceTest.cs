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
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
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
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn3"
            };
            var record2 = new TestRecord
            {
                Column1 = "Column3",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
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
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };
            var record2 = new TestRecord
            {
                Column1 = "Column2",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
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
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
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
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            await Task.Delay(1000);

            record.Column1 = "UpdatedColumn1";

            await service.UpdateRecord(id, record);

            var records = await service.GetRecords<TestRecord>();

            Assert.IsTrue(records.Item1.Exists(r => r.Column1 == record.Column1));
        }

        [TestMethod]
        public async Task UpdateExtendedRecordTest()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            record.Column1 = "UpdatedColumn1";

            await Task.Delay(1000);

            await service.UpdateRecord(id, record);

            var result1 = await service.GetRecord<TestRecord>(id);

            Assert.AreEqual(record.Column1, result1.Column1);

            record.Column1 = "UpdatedColumn1";

            await Task.Delay(1000);

            await service.UpdateRecord(id, record);

            var result2 = await service.GetRecord<ExtendedTestRecord>(id);

            Assert.AreEqual(record.Column1, result2.Column1);

            var newRecord = new ExtendedTestRecord
            {
                Id = id.ToString(),
                Column1 = record.Column1,
                Column2 = "NewColumn2",
                StringColumns = record.StringColumns,
                IntColumn = record.IntColumn,
                IntColumns = record.IntColumns,
                LongColumn = record.LongColumn,
                LongColumns = record.LongColumns,
                DoubleColumn = record.DoubleColumn,
                DoubleColumns = record.DoubleColumns,
                BoolColumn = record.BoolColumn,
                IndexId = record.IndexId,
                IndexColumn1 = record.IndexColumn1
            };

            await Task.Delay(1000);

            await service.UpdateRecord(id, newRecord);

            var result3 = await service.GetRecord<ExtendedTestRecord>(id);

            Assert.AreEqual(newRecord.Column1, result3.Column1);
            Assert.AreEqual(newRecord.Column2, result3.Column2);

            record.Column1 = "NewColumn1";

            await Task.Delay(1000);

            await service.UpdateRecord(id, record);

            var result4 = await service.GetRecord<TestRecord>(id);

            Assert.AreEqual(record.Column1, result4.Column1);

            var result5 = await service.GetRecord<ExtendedTestRecord>(id);

            Assert.AreEqual(record.Column1, result5.Column1);
            Assert.AreEqual(newRecord.Column2, result5.Column2);
        }

        [TestMethod]
        public async Task DeleteRecordByIdTest()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
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
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            await service.DeleteRecord(record);

            var records = await service.GetRecords<TestRecord>();

            Assert.IsFalse(records.Item1.Exists(r => r.Id == id.ToString()));
        }

        [TestMethod]
        public async Task AddRecordTest_EmptyStringColumns_ShouldDeleteAttribute()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string>(),
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            await service.AddRecord(record);

            var records = await service.GetRecords<TestRecord>();

            Assert.IsFalse(records.Item1.Exists(r => r.StringColumns.Count > 0));
        }

        [TestMethod]
        public async Task AddRecordTest_EmptyIntColumns_ShouldDeleteAttribute()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int>(),
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            await service.AddRecord(record);

            var records = await service.GetRecords<TestRecord>();

            Assert.IsFalse(records.Item1.Exists(r => r.IntColumns.Count > 0));
        }

        [TestMethod]
        public async Task AddRecordTest_EmptyLongColumns_ShouldDeleteAttribute()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long>(),
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            await service.AddRecord(record);

            var records = await service.GetRecords<TestRecord>();

            Assert.IsFalse(records.Item1.Exists(r => r.LongColumns.Count > 0));
        }

        [TestMethod]
        public async Task AddRecordTest_EmptyDoubleColumns_ShouldDeleteAttribute()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double>(),
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            await service.AddRecord(record);

            var records = await service.GetRecords<TestRecord>();

            Assert.IsFalse(records.Item1.Exists(r => r.DoubleColumns.Count > 0));
        }

        [TestMethod]
        public async Task UpdateRecordTest_EmptyStringColumns_ShouldDeleteAttribute()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            record.StringColumns = new List<string>();

            await service.UpdateRecord(id, record);

            var result = await service.GetRecord<TestRecord>(id);

            Assert.AreEqual(0, result.StringColumns.Count);
        }

        [TestMethod]
        public async Task UpdateRecordTest_EmptyIntColumns_ShouldDeleteAttribute()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            record.IntColumns = new List<int>();

            await service.UpdateRecord(id, record);

            var result = await service.GetRecord<TestRecord>(id);

            Assert.AreEqual(0, result.IntColumns.Count);
        }

        [TestMethod]
        public async Task UpdateRecordTest_EmptyLongColumns_ShouldDeleteAttribute()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            record.LongColumns = new List<long>();

            await service.UpdateRecord(id, record);

            var result = await service.GetRecord<TestRecord>(id);

            Assert.AreEqual(0, result.LongColumns.Count);
        }

        [TestMethod]
        public async Task UpdateRecordTest_EmptyDoubleColumns_ShouldDeleteAttribute()
        {
            var record = new TestRecord
            {
                Column1 = "Column1",
                StringColumns = new List<string> { "StringColumn1", "StringColumn2" },
                IntColumn = 1,
                IntColumns = new List<int> { 1, 2 },
                LongColumn = 1000000000000,
                LongColumns = new List<long> { 1000000000000, 2000000000000 },
                DoubleColumn = 1.1,
                DoubleColumns = new List<double> { 1.1, 2.2 },
                BoolColumn = true,
                IndexId = Guid.NewGuid().ToString(),
                IndexColumn1 = "IndexColumn1"
            };

            var id = await service.AddRecord(record);

            record.DoubleColumns = new List<double>();

            await service.UpdateRecord(id, record);

            var result = await service.GetRecord<TestRecord>(id);

            Assert.AreEqual(0, result.DoubleColumns.Count);
        }
    }

    internal class TestRecord : RecordBase
    {
        [DynamoDBProperty]
        public string Column1 { get; set; }

        [DynamoDBProperty]
        public List<string> StringColumns { get; set; } = new List<string>();

        [DynamoDBProperty]
        public int IntColumn { get; set; }

        [DynamoDBProperty]
        public List<int> IntColumns { get; set; } = new List<int>();

        [DynamoDBProperty]
        public long LongColumn { get; set; }

        [DynamoDBProperty]
        public List<long> LongColumns { get; set; } = new List<long>();

        [DynamoDBProperty]
        public double DoubleColumn { get; set; }

        [DynamoDBProperty]
        public List<double> DoubleColumns { get; set; } = new List<double>();

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
