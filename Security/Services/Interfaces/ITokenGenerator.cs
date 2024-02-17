using Security.Models;

namespace Security.Services.Interfaces {
    public interface ITokenGenerator
    {
        public string GenerateAccessToken(ApplicationUser user);
        public string GenerateRefreshToken(ApplicationUser user);
        //Task<TokenResponseModel> RefreshAccessToken(string accessToken, string refreshToken)
    }
}
