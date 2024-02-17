namespace BasketballManagerAPI.Dto.StatisticDto {
    public class PlayerStatisticDto
    {
        public string FullName { get; set; } = null!;
        public int Points { get; set; } = 0;
        public int OnePointShotHit { get; set; } = 0;
        public int OnePointShotMiss { get; set; } = 0;
        public int TwoPointShotHit { get; set; } = 0;
        public int TwoPointShotMiss { get; set; } = 0;
        public int ThreePointShotHit { get; set; } = 0;
        public int ThreePointShotMiss { get; set; } = 0;
        public int Assists { get; set; } = 0;
        public int OffensiveRebounds { get; set;  } = 0;
        public int DefensiveRebounds { get; set; } = 0; 
        public int Steals { get; set; } = 0;
        public int Blocks { get; set; } = 0;
        public int Turnovers { get; set; } = 0;
        public TimeSpan CourtTime { get; set; }

    }
}
