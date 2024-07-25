using GoalsClient.Data.Entities;
using static GoalsClient.Services.TaskItemService.TaskItemsService;

namespace GoalsClient.Services.TaskItemService
{
    public interface ITaskItemsService
    {
        Task<List<TaskItem>> GetTaskItems();

        Task<TaskItem> GetTaskItemById(int id);

        Task<TaskItemCreationResult> CreateTaskItem(TaskItem taskItem);

        Task<TaskItemUpdateResult> UpdateTaskItem(TaskItem taskItem);

        Task<bool> DeleteTaskItem(int id);
    }
}
