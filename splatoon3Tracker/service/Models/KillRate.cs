﻿using Nagiyu.Splatoon3Tracker.Service.Consts;
using System;

namespace Nagiyu.Splatoon3Tracker.Service.Models
{
    /// <summary>
    /// KillRate
    /// </summary>
    public class KillRate
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Battle
        /// </summary>
        public Splatoon3Enums.BattleType Battle { get; set; }

        /// <summary>
        /// Rule
        /// </summary>
        public Splatoon3Enums.RuleType Rule { get; set; }

        /// <summary>
        /// Weapon
        /// </summary>
        public Splatoon3Enums.Weapon Weapon { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        public Splatoon3Enums.ResultType Result { get; set; }

        /// <summary>
        /// Kill
        /// </summary>
        public int Kill { get; set; }

        /// <summary>
        /// Assist
        /// </summary>
        public int Assist { get; set; }

        /// <summary>
        /// Death
        /// </summary>
        public int Death { get; set; }

        /// <summary>
        /// Special
        /// </summary>
        public int Special { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// MatchTime
        /// </summary>
        public int MatchTime { get; set; }
    }
}
