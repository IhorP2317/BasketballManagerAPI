using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.OrderDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;

namespace BasketballManagerAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IMatchService _matchService;
        private readonly ITicketService _ticketService; 
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context, IUserService userService, IMapper mapper, IMatchService matchService, ITicketService ticketService)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _matchService = matchService;
            _ticketService = ticketService;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersByUserIdAsync(Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (!await _userService.IsUserExistsAsync(userId, cancellationToken))
                throw new NotFoundException($"User with id {userId} does not exist!");
            var orders = await _context.Orders.AsNoTracking().Include(o => o.Tickets).Where(o => o.UserId == userId)
                .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<OrderResponseDto> GetOrderAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.AsNoTracking().Include(o => o.Tickets)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
            if (order == null)
                throw new NotFoundException($"Order with id {id} does not exist!");
            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<bool> IsOrderExistAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto orderRequestDto,
            CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(orderRequestDto.UserId, cancellationToken);
            if(user == null)
                throw new NotFoundException($"User with id {orderRequestDto.UserId} does not exist!");

            await ValidateOrderAsync(orderRequestDto, cancellationToken);
           var order = _mapper.Map<Order>(orderRequestDto);
           if (orderRequestDto.Tickets.Any())
           {
               order.Tickets = _mapper.Map<ICollection<Ticket>>(orderRequestDto.Tickets);
               foreach (var ticket in order.Tickets)
               {
                   ticket.Price = _ticketService.CalculateTicketPrice(ticket.Section, ticket.Row, ticket.Seat);
               }

           }

           order.TotalPrice = order.Tickets.Sum(t => t.Price);
           if (order.TotalPrice > user.Balance)
               throw new DomainLogicException($"User does not enough balance to make order!");
           user.Balance -= order.TotalPrice;
           var createdOrder = await _context.AddAsync(order, cancellationToken);
           await _context.SaveChangesAsync(cancellationToken);
           return _mapper.Map<OrderResponseDto>(createdOrder.Entity);

        }

        public async Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders
                .Include(o => o.Tickets)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
            if (order == null)
                throw new NotFoundException($"Order with id {id} does not exist!");
            var user = await _context.Users.FindAsync(order.UserId, cancellationToken);
            var matchId = order.Tickets.FirstOrDefault()?.MatchId;
            if (matchId != null)
            {
                var match = await _context.Matches.FindAsync(matchId, cancellationToken);
                if (match != null && match.Status == MatchStatus.Scheduled)
                    user.Balance += order.TotalPrice;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateOrderAsync(OrderRequestDto orderRequest,
            CancellationToken cancellationToken)
        {
            var uniqueMatchIds = orderRequest.Tickets.Select(t => t.MatchId).Distinct().Count();
            if (uniqueMatchIds > 1)
                throw new DomainLogicException("All tickets must be for the same match.");
            
            var uniqueCombinations = orderRequest.Tickets
                .GroupBy(t => new { t.Section, t.Row, t.Seat })
                .All(g => g.Count() == 1);

            if (!uniqueCombinations)
                throw new DomainLogicException("Each ticket must have a unique combination of section, row, and seat.");
            

            var matchId = orderRequest.Tickets.First().MatchId;
            var match = await _matchService.GetMatchAsync(matchId,cancellationToken);
            if (match == null)
                throw new NotFoundException($"Match with id {matchId} does not exist!");
            if (match.Status.ToLower() != MatchStatus.Scheduled.ToString().ToLower())
                throw new DomainLogicException("Can not procedure tickets for already started or completed match!");

            foreach (var ticketRequest in orderRequest.Tickets)
            {
                var ticketSold = await _context.Tickets.AnyAsync(
                    t => t.MatchId == matchId &&
                         t.Section == ticketRequest.Section &&
                         t.Row == ticketRequest.Row &&
                         t.Seat == ticketRequest.Seat,
                    cancellationToken);

                if (ticketSold)
                {
                    throw new DomainUniquenessException("Ticket",
                        $" Match id {matchId}, Section {ticketRequest.Section}, Row {ticketRequest.Row}, Seat {ticketRequest.Seat} ");
                }
                var isOutOfCapacity = ticketRequest.Row > match.RowCount || ticketRequest.Seat > match.SeatCount || ticketRequest.Section > match.SectionCount;
                if( isOutOfCapacity )
                    throw new DomainLogicException("Ticket can not take place outside the capacity of the court!");
            }
        }
    }
}
