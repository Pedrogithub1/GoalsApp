using AutoMapper;
using GoalsAPI.Data.Entities;
using GoalsAPI.Models.GoalDtos;
using GoalsAPI.Models.TaskItemDtos;

namespace SupervisorMobility.API.Profiles
{
    public class TaskItemProfile : Profile
    {
        public TaskItemProfile()
        {
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<TaskItem, TaskItemForCreationDto>().ReverseMap();
            CreateMap<TaskItem, TaskItemForUpdateDto>().ReverseMap();
        }
    }
}
