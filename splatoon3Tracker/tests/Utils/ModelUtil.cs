using Nagiyu.Splatoon3Tracker.Service.Consts;
using Nagiyu.Splatoon3Tracker.Service.Exceptions;
using Nagiyu.Splatoon3Tracker.Service.Models;
using Nagiyu.Splatoon3Tracker.Service.Models.DB;
using Nagiyu.Splatoon3Tracker.Service.Models.Requests;
using Nagiyu.Splatoon3Tracker.Service.Models.Responses;
using System;
using System.Globalization;

namespace Nagiyu.Splatoon3Tracker.Service.Utils
{
    /// <summary>
    /// Model 用 Util
    /// </summary>
    public static class ModelUtil
    {
        /// <summary>
        /// KillRateRequest を KillRate に変換
        /// </summary>
        /// <param name="userId">UserID</param>
        /// <param name="request">KillRateRequest</param>
        /// <returns>KillRate</returns>
        public static KillRate ConvertToKillRate(Guid id, Guid userId, KillRateRequest request)
        {
            if (!Enum.TryParse(request.Battle, out Splatoon3Enums.BattleType battle))
            {
                throw new ParameterException("Battle is invalid.");
            }

            if (!Enum.TryParse(request.Rule, out Splatoon3Enums.RuleType rule))
            {
                throw new ParameterException("Rule is invalid.");
            }

            if (!Enum.TryParse(request.Weapon, out Splatoon3Enums.Weapon weapon))
            {
                throw new ParameterException("Weapon is invalid.");
            }

            if (!Enum.TryParse(request.Result, out Splatoon3Enums.ResultType result))
            {
                throw new ParameterException("Result is invalid.");
            }

            var dateFormats = new string[] { "yyyy-MM-dd HH:mm", "yyyy/MM/dd HH:mm" };
            if (!DateTime.TryParseExact(request.Date, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                throw new ParameterException("Date is invalid. Accepted formats: yyyy-MM-dd HH:mm, yyyy/MM/dd HH:mm.");
            }

            return new KillRate
            {
                Id = id,
                UserId = userId,
                Battle = battle,
                Rule = rule,
                Weapon = weapon,
                Result = result,
                Kill = request.Kill,
                Assist = request.Assist,
                Death = request.Death,
                Special = request.Special,
                Date = date,
                MatchTime = request.MatchTime
            };
        }

        /// <summary>
        /// KillRateRecord を KillRate に変換
        /// </summary>
        /// <param name="record">KillRateRecord</param>
        /// <returns>KillRate</returns>
        public static KillRate ConvertToKillRate(KillRateRecord record)
        {
            if (!Guid.TryParse(record.Id, out var id))
            {
                throw new ParameterException("Id is invalid.");
            }

            if (!Guid.TryParse(record.UserId, out var userId))
            {
                throw new ParameterException("UserId is invalid.");
            }

            if (!Enum.TryParse(record.Battle, out Splatoon3Enums.BattleType battle))
            {
                throw new ParameterException("Battle is invalid.");
            }

            if (!Enum.TryParse(record.Rule, out Splatoon3Enums.RuleType rule))
            {
                throw new ParameterException("Rule is invalid.");
            }

            if (!Enum.TryParse(record.Weapon, out Splatoon3Enums.Weapon weapon))
            {
                throw new ParameterException("Weapon is invalid.");
            }

            if (!Enum.TryParse(record.Result, out Splatoon3Enums.ResultType result))
            {
                throw new ParameterException("Result is invalid.");
            }

            var dateFormats = new string[] { "yyyy-MM-dd HH:mm", "yyyy/MM/dd HH:mm" };
            if (!DateTime.TryParseExact(record.Date, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                throw new ParameterException("Date is invalid. Accepted formats: yyyy-MM-dd HH:mm, yyyy/MM/dd HH:mm.");
            }

            return new KillRate
            {
                Id = id,
                UserId = userId,
                Battle = battle,
                Rule = rule,
                Weapon = weapon,
                Result = result,
                Kill = record.Kill,
                Assist = record.Assist,
                Death = record.Death,
                Special = record.Special,
                Date = date,
                MatchTime = record.MatchTime
            };
        }

        /// <summary>
        /// KillRate を KillRateRecord に変換
        /// </summary>
        /// <param name="killRate">KillRate</param>
        /// <returns>KillRateRecord</returns>
        public static KillRateRecord ConvertToKillRateRecord(KillRate killRate)
        {
            return new KillRateRecord
            {
                Id = killRate.Id.ToString(),
                RecordType = Splatoon3Enums.RecordType.KillRate.ToString(),
                UserId = killRate.UserId.ToString(),
                Battle = killRate.Battle.ToString(),
                Rule = killRate.Rule.ToString(),
                Weapon = killRate.Weapon.ToString(),
                Result = killRate.Result.ToString(),
                Kill = killRate.Kill,
                Assist = killRate.Assist,
                Death = killRate.Death,
                Special = killRate.Special,
                Date = killRate.Date.ToString("yyyy-MM-dd HH:mm"),
                MatchTime = killRate.MatchTime
            };
        }

        /// <summary>
        /// KillRate を KillRateResponse に変換
        /// </summary>
        /// <param name="killRate">KillRate</param>
        /// <returns>KillRateResponse</returns>
        public static KillRateResponse ConvertToKillRateResponse(KillRate killRate)
        {
            return new KillRateResponse
            {
                Id = killRate.Id.ToString(),
                Battle = killRate.Battle.ToString(),
                Rule = killRate.Rule.ToString(),
                Weapon = killRate.Weapon.ToString(),
                Result = killRate.Result.ToString(),
                Kill = killRate.Kill,
                Assist = killRate.Assist,
                Death = killRate.Death,
                Special = killRate.Special,
                Date = killRate.Date.ToString("yyyy-MM-dd HH:mm"),
                MatchTime = killRate.MatchTime
            };
        }
    }
}
