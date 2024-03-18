namespace BasketballManagerAPI.Dto.StatisticDto
{
    public class TotalAnnuallyStatisticDto
    {
        public int Year { get; set; }
        public double MatchCount { get; set; }
        public double Points { get; set; }
        public double OnePtShotCount { get; set; }
        public double TwoPtShotCount { get; set; }
        public double ThreePtShotCount { get; set; }
        public double OnePtShotMissCount { get; set; }
        public double TwoPtShotMissCount { get; set; }
        public double ThreePtShotMissCount { get; set; }
        public double AssistCount { get; set; }
        public double OffensiveReboundCount { get; set; }
        public double DefensiveReboundCount { get; set; }
        public double StealCount { get; set; }
        public double BlockCount { get; set; }
        public double TurnOverCount { get; set; }
        public TimeSpan CourtTime { get; set; }
        public double OnePointShotPercentage { get; set; } 
        public double TwoPointShotPercentage { get; set; } 
        public double ThreePointShotPercentage { get; set; } 

    }
}
