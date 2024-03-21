using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Dto.TicketDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;

namespace BasketballManagerAPI.Services.Implementations {
    public class TicketService : ITicketService {
        private readonly ApplicationDbContext _context;
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private const decimal TICKET_MAX_PRICE = 1000m;
        public TicketService(ApplicationDbContext context, IMatchService matchService, IUserService userService, IMapper mapper) {
            _context = context;
            _matchService = matchService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<PagedList<TicketResponseDto>> GetAllTicketsByMatchIdAsync(Guid matchId,
            TicketFiltersDto ticketFiltersDto,
            CancellationToken cancellationToken = default) {
            if (!await _matchService.IsMatchExist(matchId, cancellationToken))
                throw new NotFoundException($"Match with id {matchId} does not exist! ");
            IQueryable<Ticket> ticketsQuery = _context.Tickets.AsNoTracking().Where(t => t.MatchId == matchId);
            ticketsQuery = ApplyFilters(ticketsQuery, ticketFiltersDto);
            ticketsQuery = ticketFiltersDto.SortOrder?.ToLower() == "desc"
                ? ticketsQuery.OrderByDescending(t => t.Price)
                : ticketsQuery.OrderBy(t => t.Price);
            var tickets = await PagedList<Ticket>.CreateAsync(
                ticketsQuery,
                ticketFiltersDto.Page,
                ticketFiltersDto.PageSize,
                cancellationToken);
            return _mapper.Map<PagedList<TicketResponseDto>>(tickets);
        }

        public async Task<PagedList<TicketResponseDto>> GetAllTicketsByUserIdAsync(Guid userId,
            TicketFiltersDto ticketFiltersDto,
            CancellationToken cancellationToken = default) {
            if (!await _userService.IsUserExistsAsync(userId, cancellationToken))
                throw new NotFoundException($"User with id {userId} does not exist! ");
            IQueryable<Ticket> ticketsQuery = _context.Tickets
                .AsNoTracking()
                .Include(t => t.Order)
                .Where(t => t.Order.UserId == userId);

            ticketsQuery = ApplyFilters(ticketsQuery, ticketFiltersDto);
            ticketsQuery = ticketFiltersDto.SortOrder?.ToLower() == "desc"
                ? ticketsQuery.OrderByDescending(t => t.Price)
                : ticketsQuery.OrderBy(t => t.Price);

            var tickets = await PagedList<Ticket>.CreateAsync(
                ticketsQuery,
                ticketFiltersDto.Page,
                ticketFiltersDto.PageSize,
                cancellationToken);
            return _mapper.Map<PagedList<TicketResponseDto>>(tickets);
        }

        public async Task<TicketResponseDto> GetTicketAsync(Guid id, CancellationToken cancellationToken = default) {
            var ticket = await _context.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (ticket == null)
                throw new NotFoundException($"Ticket with id {id} does not exist!");
            return _mapper.Map<TicketResponseDto>(ticket);
        }

        public async Task<bool> IsTicketExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.Tickets.AnyAsync(t => t.Id == id, cancellationToken);
        }

        private IQueryable<Ticket> ApplyFilters(IQueryable<Ticket> query, TicketFiltersDto ticketFiltersDto) {
            if (ticketFiltersDto.MinPrice.HasValue)
                query = query.Where(t => t.Price >= ticketFiltersDto.MinPrice.Value);
            if (ticketFiltersDto.MaxPrice.HasValue)
                query = query.Where(t => t.Price <= ticketFiltersDto.MaxPrice.Value);
            if (ticketFiltersDto.Section.HasValue)
                query = query.Where(t => t.Section == ticketFiltersDto.Section.Value);
            if (ticketFiltersDto.Row.HasValue)
                query = query.Where(t => t.Row == ticketFiltersDto.Row.Value);
            if (ticketFiltersDto.Seat.HasValue)
                query = query.Where(t => t.Seat == ticketFiltersDto.Seat.Value);
            return query;

        }
        public decimal CalculateTicketPrice(int section, int row, int seat) {

            return TICKET_MAX_PRICE - (section * 2 + row + seat * 0.5m);
        }
    }
}
