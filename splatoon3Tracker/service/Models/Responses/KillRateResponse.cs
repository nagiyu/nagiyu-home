namespace Nagiyu.Splatoon3Tracker.Service.Models.Responses
{
    /// <summary>
    /// レスポンス用の KillRate
    /// </summary>
    public class KillRateResponse
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Battle
        /// </summary>
        public string Battle { get; set; }

        /// <summary>
        /// Rule
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// Weapon
        /// </summary>
        public string Weapon { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        public string Result { get; set; }

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
        public string Date { get; set; }

        /// <summary>
        /// MatchTime
        /// </summary>
        public int MatchTime { get; set; }
    }
}
