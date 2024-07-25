using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GoalsAPI.Data.Entities
{
    public class Goal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalId { get; set; }
        [Required]
        [MaxLength(80)]
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public int TotalTasks { get; set; }

        private ICollection<TaskItem> _taskItems = new List<TaskItem>();

        public ICollection<TaskItem> TaskItems
        {
            get => _taskItems;
            set
            {
                _taskItems = value;
                UpdateTotalTasks();
            }
        }

        public Goal(string name)
        {
            Name = name;
        }

        private void UpdateTotalTasks()
        {
            TotalTasks = _taskItems.Count;
        }

        public void AddTaskItem(TaskItem taskItem)
        {
            _taskItems.Add(taskItem);
            UpdateTotalTasks();
        }

        public void RemoveTaskItem(TaskItem taskItem)
        {
            _taskItems.Remove(taskItem);
            UpdateTotalTasks();
        }

    }
}
