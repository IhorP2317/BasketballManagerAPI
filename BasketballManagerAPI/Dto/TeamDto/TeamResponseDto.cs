namespace BasketballManagerAPI.Dto.TeamDto {
    public class TeamResponseDto: BaseEntityResponseDto {
        public string Name { get; set; } = null!;
        public string Logo { get; set; } = null!;
    }
}
