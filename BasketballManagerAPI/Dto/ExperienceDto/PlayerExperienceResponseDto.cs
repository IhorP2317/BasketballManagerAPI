namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class PlayerExperienceResponseDto {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
