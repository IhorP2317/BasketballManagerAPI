namespace BasketballManagerAPI.Dto.StatisticDto
{
    public class TotalAnnuallyStatisticDto
    {
        public int Year { get; set; }
        public int MatchCount { get; set; }
        public int Points { get; set; }
        public int OnePtShotCount { get; set; }
        public int TwoPtShotCount { get; set; }
        public int ThreePtShotCount { get; set; }
        public int OnePtShotMissCount { get; set; }
        public int TwoPtShotMissCount { get; set; }
        public int ThreePtShotMissCount { get; set; }
        public int AssistCount { get; set; }
        public int OffensiveReboundCount { get; set; }
        public int DefensiveReboundCount { get; set; }
        public int StealCount { get; set; }
        public int BlockCount { get; set; }
        public int TurnOverCount { get; set; }
        public TimeSpan CourtTime { get; set; }

    }
}
