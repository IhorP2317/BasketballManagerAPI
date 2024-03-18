namespace BasketballManagerAPI.Dto.StatisticDto {
    public class MatchTeamStaticDto
    {
        public string Name { get; set; } = null!;
        public IEnumerable<PlayerStatisticDto> Statistics { get; set; } = null!;

    }
}
