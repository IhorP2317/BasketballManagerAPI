namespace BasketballManagerAPI.Dto.StatisticDto {
    public class MatchTeamStatisticDto
    {
        public string Name { get; set; } = null!;
        public IEnumerable<PlayerStatisticDto> Statistics { get; set; } = null!;

    }
}
