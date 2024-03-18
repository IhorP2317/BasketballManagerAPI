using BasketballManagerAPI.Dto.PlayerDto;

namespace BasketballManagerAPI.Dto.AwardDto {
    public class AwardResponseDto:BaseEntityResponseDto {
        public string Name { get; set; } = null!;
        public DateOnly Date { get; set; }
        public bool IsIndividualAward { get; set; }
        public string? PhotoPath { get; set; }

    }
}
