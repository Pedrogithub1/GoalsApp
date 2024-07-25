namespace GoalsClient.Data.Entities
{
    public class Goal
    {
        public int GoalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int TotalTasks { get; set; }

        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

    }
}
