using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Nagiyu.Splatoon3Tracker.Service.Consts;
using System;
using System.Collections.Generic;

namespace Nagiyu.Splatoon3Tracker.Service.Models.DB
{
    public class KillRate : ObjectBase
    {
        private string _userId;

        [DynamoDBProperty]
        public string UserId
        {
            get => _userId;
            set
            {
                if (Guid.TryParse(value, out var battleType))
                {
                    _userId = battleType.ToString();
                }
                else
                {
                    _userId = string.Empty;
                }
            }
        }

        private string _battle;

        [DynamoDBProperty]
        public string Battle
        {
            get => _battle;
            set
            {
                if (Enum.TryParse<Splatoon3Enums.BattleType>(value, out var battleType))
                {
                    _battle = battleType.ToString();
                }
                else
                {
                    _battle = string.Empty;
                }
            }
        }

        private string _rule;

        [DynamoDBProperty]
        public string Rule
        {
            get => _rule;
            set
            {
                if (Enum.TryParse<Splatoon3Enums.RuleType>(value, out var ruleType))
                {
                    _rule = ruleType.ToString();
                }
                else
                {
                    _rule = string.Empty;
                }
            }
        }

        private string _weapon;

        [DynamoDBProperty]
        public string Weapon
        {
            get => _weapon;
            set
            {
                if (Enum.TryParse<Splatoon3Enums.Weapon>(value, out var weapon))
                {
                    _weapon = weapon.ToString();
                }
                else
                {
                    _weapon = string.Empty;
                }
            }
        }

        private string _result;

        [DynamoDBProperty]
        public string Result
        {
            get => _result;
            set
            {
                if (Enum.TryParse<Splatoon3Enums.ResultType>(value, out var resultType))
                {
                    _result = resultType.ToString();
                }
                else
                {
                    _result = string.Empty;
                }
            }
        }

        [DynamoDBProperty]
        public int Kill { get; set; }

        [DynamoDBProperty]
        public int Assist { get; set; }

        [DynamoDBProperty]
        public int Death { get; set; }

        [DynamoDBProperty]
        public int Special { get; set; }

        [DynamoDBProperty]
        public DateTime Date { get; set; }

        [DynamoDBProperty]
        public double MatchTime { get; set; }

        public KillRate() : base()
        {
        }

        public KillRate(Dictionary<string, AttributeValue> keyValuePairs) : base(keyValuePairs)
        {
            if (keyValuePairs.TryGetValue(nameof(UserId), out var userId))
            {
                UserId = userId.S;
            }
            else
            {
                UserId = string.Empty;
            }

            if (keyValuePairs.TryGetValue(nameof(Battle), out var battleType))
            {
                Battle = battleType.S;
            }
            else
            {
                Battle = string.Empty;
            }

            if (keyValuePairs.TryGetValue(nameof(Rule), out var ruleType))
            {
                Rule = ruleType.S;
            }
            else
            {
                Rule = string.Empty;
            }

            if (keyValuePairs.TryGetValue(nameof(Weapon), out var weapon))
            {
                Weapon = weapon.S;
            }
            else
            {
                Weapon = string.Empty;
            }

            if (keyValuePairs.TryGetValue(nameof(Result), out var resultType))
            {
                Result = resultType.S;
            }
            else
            {
                Result = string.Empty;
            }

            if (keyValuePairs.TryGetValue(nameof(Kill), out var kill) && int.TryParse(kill.N, out var killCount))
            {
                Kill = killCount;
            }
            else
            {
                Kill = 0;
            }

            if (keyValuePairs.TryGetValue(nameof(Assist), out var assist) && int.TryParse(assist.N, out var assistCount))
            {
                Assist = assistCount;
            }
            else
            {
                Assist = 0;
            }

            if (keyValuePairs.TryGetValue(nameof(Death), out var death) && int.TryParse(death.N, out var deathCount))
            {
                Death = deathCount;
            }
            else
            {
                Death = 0;
            }

            if (keyValuePairs.TryGetValue(nameof(Special), out var special) && int.TryParse(special.N, out var specialCount))
            {
                Special = specialCount;
            }
            else
            {
                Special = 0;
            }

            if (keyValuePairs.TryGetValue(nameof(Date), out var date) && DateTime.TryParse(date.S, out var dateTime))
            {
                Date = dateTime;
            }
            else
            {
                Date = DateTime.Now;
            }

            if (keyValuePairs.TryGetValue(nameof(MatchTime), out var matchTime) && double.TryParse(matchTime.N, out var matchTimeValue))
            {
                MatchTime = matchTimeValue;
            }
            else
            {
                MatchTime = 0;
            }
        }
    }
}
