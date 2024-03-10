using Security.Dto;
using Security.Models;

namespace Security.Services.Interfaces {
    public interface ITokenGenerator
    {
        public Task<string> GenerateAccessToken(ApplicationUser user);
        public string GenerateRefreshToken(ApplicationUser user);
       
    }
}
