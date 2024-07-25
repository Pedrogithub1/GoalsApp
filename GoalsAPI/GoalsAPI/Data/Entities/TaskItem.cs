using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GoalsAPI.Data.Entities
{
    public class TaskItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskItemId { get; set; }
        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;
        
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;

        public int GoalId { get; set; }
        public Goal? Goal { get; set; }

        public TaskItem(string name)
        {
            Name = name;
        }

    }
}
