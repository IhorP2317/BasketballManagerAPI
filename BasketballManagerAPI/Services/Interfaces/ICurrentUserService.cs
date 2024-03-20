namespace BasketballManagerAPI.Services.Interfeces {
    public interface ICurrentUserService {
        string? AccessTokenRaw { get; }
        string? UserId { get; }
        string? Email { get; }
        string? UserRole { get; }
    }
}
