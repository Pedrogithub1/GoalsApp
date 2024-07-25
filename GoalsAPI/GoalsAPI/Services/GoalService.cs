using AutoMapper;
using GoalsAPI.Data;
using GoalsAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoalsAPI.Services
{
    public class GoalService : IGoalService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public GoalService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region GoalOperations
        public async Task<IEnumerable<Goal>> GetGoalsAsync(bool inlcudeTaskItems = false)
        {

            var query = _context.Goals.AsQueryable();
            if (inlcudeTaskItems)
            {
                query = query
                 .Include(p => p.TaskItems);
            }

            return await query.ToListAsync();
        }

        public async Task<Goal?> GetGoalAsync(int goalId, bool inlcudeTaskItems = false)
        {
            var query = _context.Goals.Where(g => g.GoalId == goalId);

            if (inlcudeTaskItems)
            {
                query = query
                 .Include(p => p.TaskItems);
            }

            return await _context.Goals
                .Where(c => c.GoalId == goalId).FirstOrDefaultAsync();
        }

        public async Task<bool> GoalExistAsync(int goalId)
        {
            return await _context.Goals.AnyAsync(p => p.GoalId == goalId);
        }

        public async Task<bool> GoalNameExistsAsync(string name)
        {
            return await _context.Goals.AnyAsync(g => g.Name == name);
        }

        public void AddGoal(Goal goal)
        {
            _context.Goals.Add(goal);
        }

        public void DeleteGoal(Goal goal)
        {
            _context.Goals.Remove(goal);
            _context.SaveChanges();
        }
        #endregion





        #region TaskItemOperations
        public async Task<IEnumerable<TaskItem>> GetTaskItemsAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem?> GetTaskItemAsync(int taskItemId)
        {
            return await _context.TaskItems
                .Where(c => c.TaskItemId == taskItemId).FirstOrDefaultAsync();
        }

        public async Task<bool> TaskItemExistAsync(int taskItemId)
        {
            return await _context.TaskItems.AnyAsync(p => p.TaskItemId == taskItemId);
        }

        public async Task<bool> TaskItemNameExistsAsync(string name)
        {
            return await _context.TaskItems.AnyAsync(g => g.Name == name);
        }


        public void AddTaskItem(TaskItem taskItem)
        {
            _context.TaskItems.Add(taskItem);
        }

        public void DeleteTaskItem(TaskItem taskItem)
        {
            _context.TaskItems.Remove(taskItem);
            _context.SaveChanges();
        }
        #endregion

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }


    }

}