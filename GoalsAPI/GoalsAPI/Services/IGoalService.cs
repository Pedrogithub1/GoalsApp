using GoalsAPI.Data.Entities;

namespace GoalsAPI.Services
{
    public interface IGoalService
    {

        #region GoalsOperations
        Task<IEnumerable<Goal>> GetGoalsAsync(bool inlcudeTaskItems = false);
        Task<Goal?> GetGoalAsync(int goalId, bool inlcudeTaskItems = false);
        Task<bool> GoalExistAsync(int goalId);
        Task<bool> GoalNameExistsAsync(string name);
        void AddGoal(Goal goal);
        void DeleteGoal(Goal goal);
        #endregion


        #region TaskItemsOperations
        Task<IEnumerable<TaskItem>> GetTaskItemsAsync();
        Task<TaskItem?> GetTaskItemAsync(int taskItemId);
        Task<bool> TaskItemExistAsync(int taskItemId);
        Task<bool> TaskItemNameExistsAsync(string name);

        void AddTaskItem(TaskItem taskItem);
        void DeleteTaskItem(TaskItem taskItem);
        #endregion
        Task<bool> SaveChangesAsync();
    }
}
