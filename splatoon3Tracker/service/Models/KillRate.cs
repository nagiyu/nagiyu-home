using System;

namespace Nagiyu.Splatoon3Tracker.Service.Models
{
    public class KillRate
    {
        public Guid Id { get; set; }

        public string RecordType { get; set; }

        public string Battle { get; set; }

        public string Rule { get; set; }

        public string Weapon { get; set; }

        public string Result { get; set; }

        public int Kill { get; set; }

        public int Assist { get; set; }

        public int Death { get; set; }

        public int Special { get; set; }

        public DateTime Date { get; set; }

        public double MatchTime { get; set; }
    }
}
