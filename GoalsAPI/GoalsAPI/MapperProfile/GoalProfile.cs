using AutoMapper;
using GoalsAPI.Data.Entities;
using GoalsAPI.Models.GoalDtos;

namespace SupervisorMobility.API.Profiles
{
    public class GoalProfile : Profile
    {
        public GoalProfile()
        {
            CreateMap<Goal, GoalDto>();
            CreateMap<Goal, GoalForCreationDto>().ReverseMap();
            CreateMap<Goal, GoalForUpdateDto>().ReverseMap();
        }
    }
}
