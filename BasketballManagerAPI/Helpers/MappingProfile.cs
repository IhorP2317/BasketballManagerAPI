using System;
using System.Globalization;
using AutoMapper;
using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Dto.MatchDto;
using BasketballManagerAPI.Dto.PlayerDto;

using BasketballManagerAPI.Dto.StatisticDto;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Dto.TicketDto;
using BasketballManagerAPI.Dto.TransactionDto;
using BasketballManagerAPI.Dto.UserDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Helpers {
    public class MappingProfile : Profile {
        public MappingProfile()
        {

            ConfigureTeamEntityMapping();
            ConfigureTicketEntityMapping();
            ConfigureAwardEntityMapping();
            ConfigureMatchEntityMapping();
            ConfigurePlayerExperienceEntityMapping();
            ConfigureCoachExperienceEntityMapping();
            ConfigureTransactionEntityMapping();
            ConfigureUserEntityMapping();
            ConfigurePlayerEntityMapping();
            ConfigureCoachEntityMapping();
            ConfigureStatisticEntityMapping();
        }

        private void ConfigureTeamEntityMapping() {
            CreateMap<Team, TeamResponseDto>().ReverseMap();

            CreateMap<Team, TeamRequestDto>().ReverseMap();

            CreateMap<Team, TeamDetailDto>();

            CreateMap<PagedList<Team>, PagedList<TeamResponseDto>>()
                .ConvertUsing(
                    (src, dest, context) => {
                        var mappedItems = context.Mapper.Map<List<TeamResponseDto>>(src.Items);
                        return new PagedList<TeamResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                    }
                );
        }

        private void ConfigureTicketEntityMapping() {
            CreateMap<Ticket, TicketResponseDto>().ReverseMap();
            CreateMap<Ticket, TicketRequestDto>().ReverseMap();
        }

        private void ConfigureAwardEntityMapping() {
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
        }

        private void ConfigureMatchEntityMapping() {

            CreateMap<Match, MatchResponseDto>()
                .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
                .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.AwayTeam.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


            CreateMap<Match, MatchDetailDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));


            CreateMap<MatchRequestDto, Match>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(MatchStatus), src.Status, true)));

            CreateMap<PagedList<Match>, PagedList<MatchResponseDto>>()
                .ConvertUsing(
                    (src, dest, context) => {
                        var mappedItems = context.Mapper.Map<List<MatchResponseDto>>(src.Items);
                        return new PagedList<MatchResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                    }
                );

        }

        private void ConfigurePlayerExperienceEntityMapping() {
            CreateMap<PlayerExperienceRequestDto, PlayerExperience>()
                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.EndDate)
                        ? null
                        : (DateOnly?)DateOnly.Parse(src.EndDate, CultureInfo.InvariantCulture)));

            CreateMap<PlayerExperience, PlayerExperienceResponseDto>().ReverseMap();

            CreateMap<PlayerExperience, PlayerExperienceDetailDto>()
                .ForMember(
                    dest => dest.PlayerAwards,
                    opt => opt.MapFrom(src => src.PlayerAwards.Select(pa => pa.Award))
                );

            CreateMap<PagedList<PlayerExperience>, PagedList<PlayerExperienceResponseDto>>()
                .ConvertUsing(
                    (src, dest, context) => {
                        var mappedItems = context.Mapper.Map<List<PlayerExperienceResponseDto>>(src.Items);
                        return new PagedList<PlayerExperienceResponseDto>(mappedItems, src.Page, src.PageSize,
                            src.TotalCount);
                    }
                );
        }

        private void ConfigureCoachExperienceEntityMapping() {
            CreateMap<CoachExperienceRequestDto, CoachExperience>()
                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.EndDate)
                        ? null
                        : (DateOnly?)DateOnly.Parse(src.EndDate, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(CoachStatus), src.Status, true)));

            CreateMap<CoachExperience, CoachExperienceResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CoachExperienceResponseDto, CoachExperience>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(CoachStatus), src.Status, true)));

            CreateMap<CoachExperience, CoachExperienceDetailDto>()
                .ForMember(
                    dest => dest.CoachAwards,
                    opt => opt.MapFrom(src => src.CoachAwards.Select(ca => ca.Award))
                )
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }

        private void ConfigureTransactionEntityMapping() {
            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<TransactionRequestDto, Transaction>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(TransactionStatus), src.Status, true)));
        }

        private void ConfigureUserEntityMapping() {
            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<UserRequestDto, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(Role), src.Role, true)));

            CreateMap<UserSecurityResponseDto, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(Role), src.Role, true)));


            CreateMap<PagedList<User>, PagedList<UserResponseDto>>()
                .ConvertUsing(
                    (src, dest, context) => {
                        var mappedItems = context.Mapper.Map<List<UserResponseDto>>(src.Items);
                        return new PagedList<UserResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                    }
                );
        }

        private void ConfigurePlayerEntityMapping() {
            CreateMap<Player, PlayerResponseDto>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name));

            CreateMap<PlayerRequestDto, Player>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Position,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(Position), src.Position, true)));

            CreateMap<PlayerUpdateDto, Player>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Position,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(Position), src.Position, true)));

            CreateMap<Player, PlayerDetailDto>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()));

            CreateMap<PagedList<Player>, PagedList<PlayerResponseDto>>()
                .ConvertUsing(
                    (src, dest, context) => {
                        var mappedItems = context.Mapper.Map<List<PlayerResponseDto>>(src.Items);
                        return new PagedList<PlayerResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                    }
                );
        }

        private void ConfigureCoachEntityMapping() {
            CreateMap<Coach, CoachDetailDto>()
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty.ToString()))
                .ForMember(dest => dest.CoachStatus, opt => opt.MapFrom(src => src.CoachStatus.ToString()));

            CreateMap<Coach, CoachResponseDto>()
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty.ToString()))
                .ForMember(dest => dest.CoachStatus, opt => opt.MapFrom(src => src.CoachStatus.ToString()));

            CreateMap<CoachRequestDto, Coach>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Specialty,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(Specialty), src.Specialty, true)))
                .ForMember(dest => dest.CoachStatus,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(CoachStatus), src.CoachStatus, true)));

            CreateMap<CoachUpdateDto, Coach>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth, CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Specialty,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(Specialty), src.Specialty, true)))
                .ForMember(dest => dest.CoachStatus,
                    opt => opt.MapFrom(src => Enum.Parse(typeof(CoachStatus), src.CoachStatus, true)));

            CreateMap<PagedList<Coach>, PagedList<CoachResponseDto>>()
                .ConvertUsing(
                    (src, dest, context) => {
                        var mappedItems = context.Mapper.Map<List<CoachResponseDto>>(src.Items);
                        return new PagedList<CoachResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                    }
                );

        }
        private void ConfigureStatisticEntityMapping() {
            CreateMap<Statistic, StatisticDto>().ReverseMap();
        }

    }
}
