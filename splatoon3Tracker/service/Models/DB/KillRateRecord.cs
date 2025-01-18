using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Nagiyu.Splatoon3Tracker.Service.Exceptions;
using System.Collections.Generic;

namespace Nagiyu.Splatoon3Tracker.Service.Models.DB
{
    /// <summary>
    /// KillRate
    /// </summary>
    public class KillRateRecord : Splatoon3TrackerRecordBase
    {
        /// <summary>
        /// UserID
        /// </summary>
        [DynamoDBProperty]
        public string UserId { get; set; }

        /// <summary>
        /// Battle
        /// </summary>
        [DynamoDBProperty]
        public string Battle { get; set; }

        /// <summary>
        /// Rule
        /// </summary>
        [DynamoDBProperty]
        public string Rule { get; set; }

        /// <summary>
        /// Weapon
        /// </summary>
        [DynamoDBProperty]
        public string Weapon { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        [DynamoDBProperty]
        public string Result { get; set; }

        /// <summary>
        /// Kill
        /// </summary>
        [DynamoDBProperty]
        public int Kill { get; set; }

        /// <summary>
        /// Assist
        /// </summary>
        [DynamoDBProperty]
        public int Assist { get; set; }

        /// <summary>
        /// Death
        /// </summary>
        [DynamoDBProperty]
        public int Death { get; set; }

        /// <summary>
        /// Special
        /// </summary>
        [DynamoDBProperty]
        public int Special { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        [DynamoDBProperty]
        public string Date { get; set; }

        /// <summary>
        /// MatchTime
        /// </summary>
        [DynamoDBProperty]
        public int MatchTime { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KillRateRecord() : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyValuePairs">DynamoDB の AttributeValue</param>
        public KillRateRecord(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(UserId), out var userId))
            {
                UserId = userId.S;
            }
            else
            {
                throw new ParameterException("UserId is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Battle), out var battleType))
            {
                Battle = battleType.S;
            }
            else
            {
                throw new ParameterException("Battle is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Rule), out var ruleType))
            {
                Rule = ruleType.S;
            }
            else
            {
                throw new ParameterException("Rule is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Weapon), out var weapon))
            {
                Weapon = weapon.S;
            }
            else
            {
                throw new ParameterException("Weapon is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Result), out var resultType))
            {
                Result = resultType.S;
            }
            else
            {
                throw new ParameterException("Result is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Kill), out var kill))
            {
                Kill = int.Parse(kill.N);
            }
            else
            {
                throw new ParameterException("Kill is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Assist), out var assist))
            {
                Assist = int.Parse(assist.N);
            }
            else
            {
                throw new ParameterException("Assist is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Death), out var death))
            {
                Death = int.Parse(death.N);
            }
            else
            {
                throw new ParameterException("Death is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Special), out var special))
            {
                Special = int.Parse(special.N);
            }
            else
            {
                throw new ParameterException("Special is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(Date), out var date))
            {
                Date = date.S;
            }
            else
            {
                throw new ParameterException("Date is not found.");
            }

            if (keyValuePairs.TryGetValue(nameof(MatchTime), out var matchTime))
            {
                MatchTime = int.Parse(matchTime.N);
            }
            else
            {
                throw new ParameterException("MatchTime is not found.");
            }
        }
    }
}
