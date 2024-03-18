using AutoMapper;
using Security.Dto.UserDto;
using Security.Models;

namespace Security.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<UserSignUpDto, ApplicationUser>().ReverseMap();
            CreateMap<UserLoginDto, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserResponseDto>().ReverseMap();
            CreateMap<UserSignUpDto, UserResponseDto>().ReverseMap();
        }
    }
}
