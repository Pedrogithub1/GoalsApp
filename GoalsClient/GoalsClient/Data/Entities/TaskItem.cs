namespace GoalsClient.Data.Entities
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;

        public int GoalId { get; set; }
    }
}
