using GoalsAPI.Data.Entities;
using GoalsAPI.Models.TaskItemDtos;

namespace GoalsAPI.Models.GoalDtos
{
    public class GoalDto
    {
        public int GoalId { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public List<TaskItemDto> TaskItems { get; set; } = new List<TaskItemDto>();

        public int TotalTasks => TaskItems?.Count ?? 0;

    }
}
