namespace BasketballManagerAPI.Dto.StatisticDto {
    public class PlayerImpactStatisticDto {
        public Guid TeamId { get; set; }
        public string FullName { get; set; } = null!;

        public double OnePointShotMakeShare { get; set; }
        public double OnePointShotMissShare { get; set; }
        public double TwoPointShotMakeShare { get; set; }
        public double TwoPointShotMissShare { get; set; }
        public double ThreePointShotMakeShare { get; set; }
        public double ThreePointShotMissShare { get; set; }
        public double PointsShare { get; set; }
        public double AssistsShare { get; set;}
        public double OffensiveReboundsShare { get; set; }
        public double DefensiveReboundsShare { get; set; }
        public double StealsShare { get; set; }
        public double BlocksShare { get; set; }
        public double TurnoversShare { get; set; }

    }
}
