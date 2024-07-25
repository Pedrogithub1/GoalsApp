using GoalsClient.Data.Entities;
using static GoalsClient.Services.GoalService.GoalService;

namespace GoalsClient.Services.GoalService
{
    public interface IGoalService
    {
        Task<List<Goal>> GetGoals(bool includeTaskItems = false);

        Task<Goal> GetGoalById(int id, bool includeTaskItems = false);

        Task<GoalCreationResult> CreateGoal(Goal goal);

        Task<GoalUpdateResult> UpdateGoal(Goal goal);

        Task<bool> DeleteGoal(int id);
    }
}
