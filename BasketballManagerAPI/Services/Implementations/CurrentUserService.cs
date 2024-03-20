using BasketballManagerAPI.Services.Interfeces;
using System.Security.Claims;

namespace BasketballManagerAPI.Services.Implementations {
    public class CurrentUserService : ICurrentUserService {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? AccessTokenRaw => _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString().Substring("Bearer ".Length).Trim();
        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public string? UserRole =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    }
}
