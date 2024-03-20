using BasketballManagerAPI.Dto.TicketDto;

namespace BasketballManagerAPI.Services.Interfaces {
    public interface ITicketService
    {
        public Task<IEnumerable<TicketResponseDto>> GetAllTicketsByMatchIdAsync(int matchId,
            CancellationToken cancellationToken = default);

        public Task<IEnumerable<TicketResponseDto>> GetAllTicketsByUserIdAsync(int userId,
            CancellationToken cancellationToken = default);

        public Task<TicketResponseDto> GetTicketAsync(int id, CancellationToken cancellationToken = default);
        public Task<bool> IsTicketExistAsync(int id, CancellationToken cancellationToken = default);
        
    }
}
