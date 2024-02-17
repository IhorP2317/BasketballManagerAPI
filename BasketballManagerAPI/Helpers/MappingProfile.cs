using System.Globalization;
using AutoMapper;
using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Dto.MatchDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Dto.PlayerExperienceDto;
using BasketballManagerAPI.Dto.StatisticDto;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Dto.TicketDto;
using BasketballManagerAPI.Dto.TransactionDto;
using BasketballManagerAPI.Dto.UserDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Helpers {
    public class MappingProfile : Profile {
        public MappingProfile() {
           

            CreateMap<Team, TeamResponseDto>().ReverseMap();
            CreateMap<Team, TeamRequestDto>().ReverseMap();

            CreateMap<Team, TeamDetailDto>();

            CreateMap<Ticket, TicketResponseDto>().ReverseMap();
            CreateMap<Ticket, TicketRequestDto>().ReverseMap();

            CreateMap<Award, AwardResponseDto>().ReverseMap();

            CreateMap<Award, AwardRequestDto>().ForMember(dest => dest.Date,
                opt => opt.MapFrom(src => src.Date.ToString()));
            CreateMap<AwardRequestDto, Award>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.Date, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.IsIndividualAward,
                    opt => opt.MapFrom(src => src.IsIndividualAward.GetValueOrDefault()));

            CreateMap<AwardUpdateDto, Award>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.Date, CultureInfo.InvariantCulture)));


            CreateMap<Match, MatchResponseDto>()
                .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
                .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.AwayTeam.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<Match, MatchDetailDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<MatchRequestDto, Match>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse(typeof(MatchStatus), src.Status)));

            CreateMap<PlayerExperience, StaffExperienceResponseDto>()
                .ForMember(dest => dest.StaffId, options => options.MapFrom(src => src.PlayerId));
            CreateMap<StaffExperienceRequestDto,PlayerExperience>()
                .ForMember(dest => dest.PlayerId, options => options.MapFrom(src => src.StaffId));
            CreateMap<CoachExperience, StaffExperienceResponseDto>()
                .ForMember(dest => dest.StaffId, options => options.MapFrom(src => src.CoachId));
            CreateMap<StaffExperienceRequestDto, CoachExperience>()
                .ForMember(dest => dest.CoachId, options => options.MapFrom(src => src.StaffId));



            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<TransactionResponseDto, Transaction>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse(typeof(TransactionStatus), src.Status)));
            CreateMap<Transaction, TransactionRequestDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<TransactionRequestDto, Transaction>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse(typeof(TransactionStatus), src.Status)));

            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<UserResponseDto, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(Role), src.Role)));

            CreateMap<User, UserRequestDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<UserRequestDto, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(Role), src.Role)));

            CreateMap<Player, PlayerResponseDto>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name));

            CreateMap<Player, PlayerRequestDto>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString()))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()));
            CreateMap<PlayerRequestDto, Player>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Position,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(Position), src.Position)));

            CreateMap<Player, PlayerDetailDto>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
                .ForMember(dest => dest.PlayerAwards, opt => opt.MapFrom(src => src.PlayerAwards.Select(p => p.Award)));


            CreateMap<PagedList<Player>, PagedList<PlayerResponseDto>>()
                .ConvertUsing(
                    (src, dest, context) => {
                        var mappedItems = context.Mapper.Map<List<PlayerResponseDto>>(src.Items);
                        return new PagedList<PlayerResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                    }
                );
            
            CreateMap<PagedList<Match>, PagedList<MatchResponseDto>>()
                .ConvertUsing(
                    (src, dest, context) => {
                        var mappedItems = context.Mapper.Map<List<MatchResponseDto>>(src.Items);
                        return new PagedList<MatchResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                    }
                );


            CreateMap<Coach, CoachDetailDto>()
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty.ToString()))
                .ForMember(dest => dest.CoachStatus, opt => opt.MapFrom(src => src.CoachStatus.ToString()))
                .ForMember(dest => dest.CoachAwards, opt => opt.MapFrom(src => src.CoachAwards.Select(c => c.Award)));

            CreateMap<Coach, CoachResponseDto>()
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty.ToString()))
                .ForMember(dest => dest.CoachStatus, opt => opt.MapFrom(src => src.CoachStatus.ToString()));
            CreateMap<CoachResponseDto, Coach>()
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => Enum.Parse(typeof(Specialty), src.Specialty)))
                .ForMember(dest => dest.CoachStatus, opt => opt.MapFrom(src => Enum.Parse(typeof(CoachStatus), src.CoachStatus)));

            CreateMap<Coach, CoachRequestDto>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth.ToString()))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty.ToString()))
                .ForMember(dest => dest.CoachStatus, opt => opt.MapFrom(src => src.CoachStatus.ToString()));
            CreateMap<CoachRequestDto, Coach>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => Enum.Parse(typeof(Specialty), src.Specialty)))
                .ForMember(dest => dest.CoachStatus, opt => opt.MapFrom(src => Enum.Parse(typeof(CoachStatus), src.CoachStatus)));
            
            CreateMap<Statistic, StatisticDto>().ReverseMap();
        }

    }
}
