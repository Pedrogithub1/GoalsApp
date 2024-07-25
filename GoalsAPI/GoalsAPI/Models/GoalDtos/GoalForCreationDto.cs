using System.ComponentModel.DataAnnotations;

namespace GoalsAPI.Models.GoalDtos
{
    public class GoalForCreationDto
    {
        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public int TotalTasks { get; set; }
    }
}
