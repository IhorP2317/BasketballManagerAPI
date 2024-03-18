namespace BasketballManagerAPI.Services.Interfeces {
    public interface ICurrentUserService {
        string? UserId { get; }
        string? Email { get; }
        string? UserRole { get; }
    }
}
