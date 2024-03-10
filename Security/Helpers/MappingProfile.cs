using AutoMapper;
using Security.Dto;
using Security.Models;

namespace Security.Helpers {
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<UserSignUpDto, ApplicationUser>().ReverseMap();
            CreateMap<UserLoginDto, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserResponseDto>().ReverseMap();

        }
    }
}
