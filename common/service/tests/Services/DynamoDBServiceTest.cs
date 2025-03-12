using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        [TestMethod]
        public async Task GetRecordTest()
        {
            var record = new TestRecord
            {
                NonNullableString = "Test",
                NonNullableStringList = new List<string> { "Test1", "Test2" },
                NonNullableInt = 1,
                NonNullableIntList = new List<int> { 1, 2 },
                NonNullableLong = 1L,
                NonNullableLongList = new List<long> { 1L, 2L },
                NonNullableDouble = 1.0,
                NonNullableDoubleList = new List<double> { 1.0, 2.0 },
                NonNullableBool = true,
                NullableString = "NullableTest",
                NullableStringList = new List<string> { "NullableTest1", "NullableTest2" },
                NullableInt = 2,
                NullableIntList = new List<int> { 3, 4 },
                NullableLong = 2L,
                NullableLongList = new List<long> { 3L, 4L },
                NullableDouble = 2.0,
                NullableDoubleList = new List<double> { 3.0, 4.0 },
                NullableBool = false
            };

            var id = await service.AddRecord(record);

            var retrievedRecord = await service.GetRecord<TestRecord>(id);

            Assert.IsNotNull(retrievedRecord);
            Assert.AreEqual(record.NonNullableString, retrievedRecord.NonNullableString);
            Assert.AreEqual(record.NonNullableStringList.Count, retrievedRecord.NonNullableStringList.Count);
            Assert.AreEqual(record.NonNullableInt, retrievedRecord.NonNullableInt);
            Assert.AreEqual(record.NonNullableIntList.Count, retrievedRecord.NonNullableIntList.Count);
            Assert.AreEqual(record.NonNullableLong, retrievedRecord.NonNullableLong);
            Assert.AreEqual(record.NonNullableLongList.Count, retrievedRecord.NonNullableLongList.Count);
            Assert.AreEqual(record.NonNullableDouble, retrievedRecord.NonNullableDouble);
            Assert.AreEqual(record.NonNullableDoubleList.Count, retrievedRecord.NonNullableDoubleList.Count);
            Assert.AreEqual(record.NonNullableBool, retrievedRecord.NonNullableBool);
            Assert.AreEqual(record.NullableString, retrievedRecord.NullableString);
            Assert.AreEqual(record.NullableStringList.Count, retrievedRecord.NullableStringList.Count);
            Assert.AreEqual(record.NullableInt, retrievedRecord.NullableInt);
            Assert.AreEqual(record.NullableIntList.Count, retrievedRecord.NullableIntList.Count);
            Assert.AreEqual(record.NullableLong, retrievedRecord.NullableLong);
            Assert.AreEqual(record.NullableLongList.Count, retrievedRecord.NullableLongList.Count);
            Assert.AreEqual(record.NullableDouble, retrievedRecord.NullableDouble);
            Assert.AreEqual(record.NullableDoubleList.Count, retrievedRecord.NullableDoubleList.Count);
            Assert.AreEqual(record.NullableBool, retrievedRecord.NullableBool);
        }

        [TestMethod]
        public async Task GetRecord_NullableFieldsTest()
        {
            var record = new TestRecord
            {
                NonNullableString = "Test",
                NonNullableStringList = new List<string> { "Test1", "Test2" },
                NonNullableInt = 1,
                NonNullableIntList = new List<int> { 1, 2 },
                NonNullableLong = 1L,
                NonNullableLongList = new List<long> { 1L, 2L },
                NonNullableDouble = 1.0,
                NonNullableDoubleList = new List<double> { 1.0, 2.0 },
                NonNullableBool = true,
                NullableString = null,
                NullableStringList = null,
                NullableInt = null,
                NullableIntList = null,
                NullableLong = null,
                NullableLongList = null,
                NullableDouble = null,
                NullableDoubleList = null,
                NullableBool = null
            };

            var id = await service.AddRecord(record);

            var retrievedRecord = await service.GetRecord<TestRecord>(id);

            Assert.IsNotNull(retrievedRecord);
            Assert.IsNull(retrievedRecord.NullableString);
            Assert.IsNull(retrievedRecord.NullableStringList);
            Assert.IsNull(retrievedRecord.NullableInt);
            Assert.IsNull(retrievedRecord.NullableIntList);
            Assert.IsNull(retrievedRecord.NullableLong);
            Assert.IsNull(retrievedRecord.NullableLongList);
            Assert.IsNull(retrievedRecord.NullableDouble);
            Assert.IsNull(retrievedRecord.NullableDoubleList);
            Assert.IsNull(retrievedRecord.NullableBool);
        }

        [TestMethod]
        public async Task GetRecord_NonExistentIdTest()
        {
            var nonExistentId = Guid.NewGuid();

            var retrievedRecord = await service.GetRecord<TestRecord>(nonExistentId);

            Assert.IsNull(retrievedRecord);
        }

        //[TestMethod]
        //public async Task GetRecordsTest()
        //{
        //    var record1 = new TestRecord
        //    {
        //        NonNullableString = "Test1",
        //        NonNullableStringList = new List<string> { "Test1", "Test2" },
        //        NonNullableInt = 1,
        //        NonNullableIntList = new List<int> { 1, 2 },
        //        NonNullableLong = 1L,
        //        NonNullableLongList = new List<long> { 1L, 2L },
        //        NonNullableDouble = 1.0,
        //        NonNullableDoubleList = new List<double> { 1.0, 2.0 },
        //        NonNullableBool = true,
        //        NullableString = "NullableTest1",
        //        NullableStringList = new List<string> { "NullableTest1", "NullableTest2" },
        //        NullableInt = 2,
        //        NullableIntList = new List<int> { 3, 4 },
        //        NullableLong = 2L,
        //        NullableLongList = new List<long> { 3L, 4L },
        //        NullableDouble = 2.0,
        //        NullableDoubleList = new List<double> { 3.0, 4.0 },
        //        NullableBool = false
        //    };

        //    var record2 = new TestRecord
        //    {
        //        NonNullableString = "Test2",
        //        NonNullableStringList = new List<string> { "Test3", "Test4" },
        //        NonNullableInt = 3,
        //        NonNullableIntList = new List<int> { 5, 6 },
        //        NonNullableLong = 3L,
        //        NonNullableLongList = new List<long> { 5L, 6L },
        //        NonNullableDouble = 3.0,
        //        NonNullableDoubleList = new List<double> { 5.0, 6.0 },
        //        NonNullableBool = true,
        //        NullableString = "NullableTest2",
        //        NullableStringList = new List<string> { "NullableTest3", "NullableTest4" },
        //        NullableInt = 4,
        //        NullableIntList = new List<int> { 7, 8 },
        //        NullableLong = 4L,
        //        NullableLongList = new List<long> { 7L, 8L },
        //        NullableDouble = 4.0,
        //        NullableDoubleList = new List<double> { 7.0, 8.0 },
        //        NullableBool = false
        //    };

        //    await service.AddRecord(record1);
        //    await service.AddRecord(record2);

        //    var (records, nextId) = await service.GetRecords<TestRecord>();

        //    Assert.IsNotNull(records);
        //    Assert.IsTrue(records.Count >= 2);
        //}

        //[TestMethod]
        //public async Task AddRecordTest()
        //{
        //    var record = new TestRecord
        //    {
        //        NonNullableString = "Test",
        //        NonNullableStringList = new List<string> { "Test1", "Test2" },
        //        NonNullableInt = 1,
        //        NonNullableIntList = new List<int> { 1, 2 },
        //        NonNullableLong = 1L,
        //        NonNullableLongList = new List<long> { 1L, 2L },
        //        NonNullableDouble = 1.0,
        //        NonNullableDoubleList = new List<double> { 1.0, 2.0 },
        //        NonNullableBool = true,
        //        NullableString = "NullableTest",
        //        NullableStringList = new List<string> { "NullableTest1", "NullableTest2" },
        //        NullableInt = 2,
        //        NullableIntList = new List<int> { 3, 4 },
        //        NullableLong = 2L,
        //        NullableLongList = new List<long> { 3L, 4L },
        //        NullableDouble = 2.0,
        //        NullableDoubleList = new List<double> { 3.0, 4.0 },
        //        NullableBool = false
        //    };

        //    var id = await service.AddRecord(record);

        //    Assert.IsNotNull(id);
        //}

        //[TestMethod]
        //public async Task UpdateRecordTest()
        //{
        //    var record = new TestRecord
        //    {
        //        NonNullableString = "Test",
        //        NonNullableStringList = new List<string> { "Test1", "Test2" },
        //        NonNullableInt = 1,
        //        NonNullableIntList = new List<int> { 1, 2 },
        //        NonNullableLong = 1L,
        //        NonNullableLongList = new List<long> { 1L, 2L },
        //        NonNullableDouble = 1.0,
        //        NonNullableDoubleList = new List<double> { 1.0, 2.0 },
        //        NonNullableBool = true,
        //        NullableString = "NullableTest",
        //        NullableStringList = new List<string> { "NullableTest1", "NullableTest2" },
        //        NullableInt = 2,
        //        NullableIntList = new List<int> { 3, 4 },
        //        NullableLong = 2L,
        //        NullableLongList = new List<long> { 3L, 4L },
        //        NullableDouble = 2.0,
        //        NullableDoubleList = new List<double> { 3.0, 4.0 },
        //        NullableBool = false
        //    };

        //    var id = await service.AddRecord(record);

        //    record.NonNullableString = "UpdatedTest";
        //    record.NonNullableInt = 10;

        //    await service.UpdateRecord(id, record);

        //    var updatedRecord = await service.GetRecord<TestRecord>(id);

        //    Assert.IsNotNull(updatedRecord);
        //    Assert.AreEqual(record.NonNullableString, updatedRecord.NonNullableString);
        //    Assert.AreEqual(record.NonNullableInt, updatedRecord.NonNullableInt);
        //}

        //[TestMethod]
        //public async Task DeleteRecordTest()
        //{
        //    var record = new TestRecord
        //    {
        //        NonNullableString = "Test",
        //        NonNullableStringList = new List<string> { "Test1", "Test2" },
        //        NonNullableInt = 1,
        //        NonNullableIntList = new List<int> { 1, 2 },
        //        NonNullableLong = 1L,
        //        NonNullableLongList = new List<long> { 1L, 2L },
        //        NonNullableDouble = 1.0,
        //        NonNullableDoubleList = new List<double> { 1.0, 2.0 },
        //        NonNullableBool = true,
        //        NullableString = "NullableTest",
        //        NullableStringList = new List<string> { "NullableTest1", "NullableTest2" },
        //        NullableInt = 2,
        //        NullableIntList = new List<int> { 3, 4 },
        //        NullableLong = 2L,
        //        NullableLongList = new List<long> { 3L, 4L },
        //        NullableDouble = 2.0,
        //        NullableDoubleList = new List<double> { 3.0, 4.0 },
        //        NullableBool = false
        //    };

        //    var id = await service.AddRecord(record);

        //    await service.DeleteRecord(id);

        //    var deletedRecord = await service.GetRecord<TestRecord>(id);

        //    Assert.IsNull(deletedRecord);
        //}
    }

    internal class TestRecord : RecordBase
    {
        [DynamoDBProperty]
        public string NonNullableString { get; set; }

        [DynamoDBProperty]
        public List<string> NonNullableStringList { get; set; }

        [DynamoDBProperty]
        public int NonNullableInt { get; set; }

        [DynamoDBProperty]
        public List<int> NonNullableIntList { get; set; }

        [DynamoDBProperty]
        public long NonNullableLong { get; set; }

        [DynamoDBProperty]
        public List<long> NonNullableLongList { get; set; }

        [DynamoDBProperty]
        public double NonNullableDouble { get; set; }

        [DynamoDBProperty]
        public List<double> NonNullableDoubleList { get; set; }

        [DynamoDBProperty]
        public bool NonNullableBool { get; set; }

        [DynamoDBProperty]
        public string NullableString { get; set; }

        [DynamoDBProperty]
        public List<string> NullableStringList { get; set; }

        [DynamoDBProperty]
        public int? NullableInt { get; set; }

        [DynamoDBProperty]
        public List<int> NullableIntList { get; set; }

        [DynamoDBProperty]
        public long? NullableLong { get; set; }

        [DynamoDBProperty]
        public List<long> NullableLongList { get; set; }

        [DynamoDBProperty]
        public double? NullableDouble { get; set; }

        [DynamoDBProperty]
        public List<double> NullableDoubleList { get; set; }

        [DynamoDBProperty]
        public bool? NullableBool { get; set; }

        public TestRecord() : base()
        {
        }

        public TestRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            NonNullableString = DynamoDBUtil.GetStringValue(keyValuePairs, nameof(NonNullableString));
            NonNullableStringList = DynamoDBUtil.GetStringListValue(keyValuePairs, nameof(NonNullableStringList));
            NonNullableInt = DynamoDBUtil.GetIntValue(keyValuePairs, nameof(NonNullableInt)).Value;
            NonNullableIntList = DynamoDBUtil.GetIntListValue(keyValuePairs, nameof(NonNullableIntList));
            NonNullableLong = DynamoDBUtil.GetLongValue(keyValuePairs, nameof(NonNullableLong)).Value;
            NonNullableLongList = DynamoDBUtil.GetLongListValue(keyValuePairs, nameof(NonNullableLongList));
            NonNullableDouble = DynamoDBUtil.GetDoubleValue(keyValuePairs, nameof(NonNullableDouble)).Value;
            NonNullableDoubleList = DynamoDBUtil.GetDoubleListValue(keyValuePairs, nameof(NonNullableDoubleList));
            NonNullableBool = DynamoDBUtil.GetBoolValue(keyValuePairs, nameof(NonNullableBool)).Value;
            NullableString = DynamoDBUtil.GetStringValue(keyValuePairs, nameof(NullableString), false);
            NullableStringList = DynamoDBUtil.GetStringListValue(keyValuePairs, nameof(NullableStringList), false);
            NullableInt = DynamoDBUtil.GetIntValue(keyValuePairs, nameof(NullableInt), false);
            NullableIntList = DynamoDBUtil.GetIntListValue(keyValuePairs, nameof(NullableIntList), false);
            NullableLong = DynamoDBUtil.GetLongValue(keyValuePairs, nameof(NullableLong), false);
            NullableLongList = DynamoDBUtil.GetLongListValue(keyValuePairs, nameof(NullableLongList), false);
            NullableDouble = DynamoDBUtil.GetDoubleValue(keyValuePairs, nameof(NullableDouble), false);
            NullableDoubleList = DynamoDBUtil.GetDoubleListValue(keyValuePairs, nameof(NullableDoubleList), false);
            NullableBool = DynamoDBUtil.GetBoolValue(keyValuePairs, nameof(NullableBool), false);
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
