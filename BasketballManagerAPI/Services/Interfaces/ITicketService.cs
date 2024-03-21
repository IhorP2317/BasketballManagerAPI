using BasketballManagerAPI.Dto.TicketDto;
using BasketballManagerAPI.Helpers;

namespace BasketballManagerAPI.Services.Interfaces {
    public interface ITicketService
    {
        public Task<PagedList<TicketResponseDto>> GetAllTicketsByMatchIdAsync(Guid matchId,
            TicketFiltersDto ticketFiltersDto,
            CancellationToken cancellationToken = default);

        public Task<PagedList<TicketResponseDto>> GetAllTicketsByUserIdAsync(Guid userId,
            TicketFiltersDto ticketFiltersDto,
            CancellationToken cancellationToken = default);

        public Task<TicketResponseDto> GetTicketAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<bool> IsTicketExistAsync(Guid id, CancellationToken cancellationToken = default);
        
        public decimal CalculateTicketPrice(int section, int row, int seat);

    }
}
