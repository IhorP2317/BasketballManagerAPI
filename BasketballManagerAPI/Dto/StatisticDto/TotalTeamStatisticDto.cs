namespace BasketballManagerAPI.Dto.StatisticDto {
    public class TotalTeamStatisticDto {
        public string Name { get; set; } = null!;
        public int Points { get; set; } = 0;

        public int OnePointShotsCompleted { get; set; } = 0;
        public int OnePointShotsMissed { get; set; } = 0;

        public int TwoPointShotsCompleted { get; set; } = 0;
        public int TwoPointShotsMissed { get; set; } = 0;
        public int ThreePointShotsCompleted { get; set; } = 0;
        public int ThreePointShotsMissed { get; set; } = 0;
        public int Assists { get; set; } = 0;
        public int OffensiveRebounds { get; set; } = 0;
        public int DefensiveRebounds { get; set; } = 0;
        public int Steals { get; set; } = 0;
        public int Blocks { get; set; } = 0;
        public int TurnOvers { get; set; } = 0;
        public double OnePointShotPercentage { get; set; } = 0.0;
        public double TwoPointShotPercentage { get; set; } = 0.0;
        public double ThreePointShotPercentage { get; set; } = 0.0;

    }
}
