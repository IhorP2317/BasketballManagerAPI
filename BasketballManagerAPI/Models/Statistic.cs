namespace BasketballManagerAPI.Models {
    public class Statistic {
        public Guid MatchId { get; set; }
        public Match Match { get; set; } = null!;
        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public int  TimeUnit { get; set; }
        public int OnePointShotHitCount { get; set; }
        public int OnePointShotMissCount { get; set; }
        public int TwoPointShotHitCount { get; set; }
        public int TwoPointShotMissCount { get; set; }
        public int ThreePointShotHitCount { get; set; }
        public int ThreePointShotMissCount { get; set; }
        public int AssistCount { get; set; }
        public int OffensiveReboundCount { get; set; }
        public int DefensiveReboundCount { get; set; }
        public int StealCount { get; set; }
        public int BlockCount { get; set; }
        public int TurnoverCount { get; set; }
        public TimeSpan CourtTime { get; set; }
    }
}
