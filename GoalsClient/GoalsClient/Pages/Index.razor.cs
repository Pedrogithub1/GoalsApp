using GoalsClient.Data.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using static MudBlazor.CategoryTypes;

namespace GoalsClient.Pages
{
    public partial class Index
    {
        #region Variables

        // Data
        public List<Goal> _Goals { get; set; } = new();
        public List<TaskItem> _TaskItems { get; set; } = new();

        // State
        private Goal goalEdited = new();
        private TaskItem taskItemEdited = new();
        private bool isEditing = false;
        private bool isEditingTask = false;
        private string goalName = string.Empty;
        private string taskItemName = string.Empty;
        private bool showGoalDialog = false;
        private bool showTaskDialog = false;
        private List<TaskItem> selectedTaskItems = new();
        private int goalSelectedId = -1;

        // Filters
        private string taskFilter = "";
        private DateTime? dateFilter;
        private string statusFilter = "";

        private IEnumerable<TaskItem> FilteredTaskItems => _TaskItems
            .Where(task => (string.IsNullOrEmpty(taskFilter) || task.Name.Contains(taskFilter, StringComparison.OrdinalIgnoreCase))
                          && (!dateFilter.HasValue || task.Date.Date == dateFilter.Value.Date)
                          && (string.IsNullOrEmpty(statusFilter) || task.Status.Contains(statusFilter, StringComparison.OrdinalIgnoreCase)));

        // Dialog Options
        private readonly DialogOptions dialogGoalOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, CloseButton = true };
        private readonly DialogOptions dialogDeleteOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, Position = DialogPosition.TopCenter, DisableBackdropClick = true, CloseButton = true };

        // Delete Modal
        private bool visibleDelete = false;
        public int deleteGoalId = 0;
        public List<int> deleteTaskItemIds = new();
        public bool IsDeleteGoal = true;

        #endregion

        #region Lifecycle Methods

        protected async override Task OnInitializedAsync()
        {
            _Goals = await GoalServices.GetGoals(true);
            if (_Goals.Any())
            {
                goalSelectedId = _Goals.FirstOrDefault().GoalId;
                var selectedGoal = _Goals.FirstOrDefault(g => g.GoalId == goalSelectedId);
                _TaskItems = selectedGoal?.TaskItems.ToList() ?? new List<TaskItem>();
            }
        }

        #endregion

        #region Goal Methods

        public void ShowGoalDialog(bool editing, Goal? goal = null)
        {
            isEditing = editing;
            if (isEditing)
            {
                goalEdited = Clone(goal);
                goalName = goal.Name;
            }
            else
            {
                goalName = string.Empty;
            }
            showGoalDialog = true;
        }

        private void CloseGoalDialog() => showGoalDialog = false;

        private async Task CreateGoal()
        {
            Goal newGoal = new Goal
            {
                Name = goalName,
                TotalTasks = 0,
                Date = DateTime.Now
            };

            Snackbar.Clear();

            var result = await GoalServices.CreateGoal(newGoal);

            if (result.Success)
            {
                Snackbar.Add($"Meta {goalName} creada!", Severity.Success);
                await RefreshGoalsAndTasks();
                showGoalDialog = false;
            }
            else
            {
                Snackbar.Add($"Error al crear meta: {result.ErrorMessage}", Severity.Error);
            }
        }

        private async Task UpdateGoal()
        {
            Goal updateGoal = goalEdited;
            updateGoal.Name = goalName;

            Snackbar.Clear();

            var result = await GoalServices.UpdateGoal(updateGoal);

            if (result.Success)
            {
                Snackbar.Add($"Meta {updateGoal.Name} actualizada!", Severity.Success);
                await RefreshGoalsAndTasks();
                showGoalDialog = false;
            }
            else
            {
                Snackbar.Add($"Error al actualizar meta: {result.ErrorMessage}", Severity.Error);
            }
        }

        #endregion

        #region TaskItem Methods

        public void ShowTaskItemDialog(bool editing)
        {
            isEditingTask = editing;
            if (isEditingTask)
            {
                taskItemEdited = Clone(selectedTaskItems.FirstOrDefault());
                taskItemName = selectedTaskItems.FirstOrDefault()?.Name ?? string.Empty;
            }
            else
            {
                taskItemName = string.Empty;
            }
            showTaskDialog = true;
        }

        private void CloseTaskDialog() => showTaskDialog = false;

        private async Task CreateTaskItem()
        {
            TaskItem newTaskItem = new TaskItem
            {
                Name = taskItemName,
                Date = DateTime.Now,
                Status = "Abierta",
                GoalId = goalSelectedId
            };

            Snackbar.Clear();

            var result = await TaskItemsServices.CreateTaskItem(newTaskItem);

            if (result.Success)
            {
                Snackbar.Add($"Tarea {taskItemName} creada!", Severity.Success);
                await RefreshGoalsAndTasks();
                showTaskDialog = false;
                StateHasChanged();
            }
            else
            {
                Snackbar.Add($"Error al crear tarea: {result.ErrorMessage}", Severity.Error);
            }
        }

        private async Task UpdateTaskItem(bool? isCompleted = false)
        {
            if (isCompleted == true)
            {
                foreach (var taskItem in selectedTaskItems)
                {
                    taskItem.Status = "Completada";
                    var result = await TaskItemsServices.UpdateTaskItem(taskItem);
                    if (!result.Success)
                    {
                        Snackbar.Add($"Error al completar tarea: {taskItem.Name}", Severity.Error);
                        return;
                    }
                }
                Snackbar.Add("Tarea(s) completada(s)!", Severity.Success);
            }
            else
            {
                TaskItem updateTaskItem = taskItemEdited;
                updateTaskItem.Name = taskItemName;

                Snackbar.Clear();

                var result = await TaskItemsServices.UpdateTaskItem(updateTaskItem);

                if (result.Success)
                {
                    Snackbar.Add($"Tarea {updateTaskItem.Name} actualizada!", Severity.Success);
                }
                else
                {
                    Snackbar.Add($"Error al actualizar tarea: {result.ErrorMessage}", Severity.Error);
                }
            }
            await RefreshGoalsAndTasks();
            showTaskDialog = false;
            selectedTaskItems.Clear();
            StateHasChanged();
        }

        #endregion

        #region Delete Methods

        private async Task DeleteGoalOrTasks()
        {
            if (IsDeleteGoal)
            {
                bool response = await GoalServices.DeleteGoal(deleteGoalId);
                if (response)
                {
                    Snackbar.Add("Meta eliminada!", Severity.Success);
                    await RefreshGoalsAndTasks();
                    showTaskDialog = false;
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add("Error al eliminar meta!", Severity.Error);
                }
            }
            else
            {
                bool response = true;
                foreach (var id in deleteTaskItemIds)
                {
                    response = await TaskItemsServices.DeleteTaskItem(id);
                    if (!response) break;
                }
                if (response)
                {
                    Snackbar.Add("Tarea(s) eliminada(s)!", Severity.Success);
                    await RefreshGoalsAndTasks();
                    showTaskDialog = false;
                    selectedTaskItems.Clear();
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add("Error al eliminar tarea(s)!", Severity.Error);
                }
            }

            visibleDelete = false;
        }

        private void OpenDeleteDialog(List<int> deleteIds, bool isGoal = true)
        {
            IsDeleteGoal = isGoal;
            if (isGoal)
            {
                deleteGoalId = deleteIds.FirstOrDefault();
            }
            else
            {
                deleteTaskItemIds = deleteIds;
            }
            visibleDelete = true;
        }

        private void CloseDeleteModal() => visibleDelete = false;

        #endregion

        #region Helper Methods

        private void SelectGoal(int index)
        {
            goalSelectedId = index;
            UpdateTaskItems();
        }

        private void UpdateTaskItems()
        {
            var selectedGoal = _Goals.FirstOrDefault(g => g.GoalId == goalSelectedId);
            _TaskItems = selectedGoal?.TaskItems.ToList() ?? new List<TaskItem>();
        }

        private void SelectTaskItem(TaskItem taskItem)
        {
            if (selectedTaskItems.Contains(taskItem))
            {
                selectedTaskItems.Remove(taskItem);
            }
            else
            {
                selectedTaskItems.Add(taskItem);
            }
        }

        private bool IsSingleTaskItemSelected => selectedTaskItems.Count == 1;
        private bool AreMultipleTaskItemsSelected => selectedTaskItems.Count > 1;
        private bool IsAnyTaskItemSelected => selectedTaskItems.Count > 0;

        private T Clone<T>(T source) => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source));

        private async Task RefreshGoalsAndTasks()
        {
            _Goals.Clear();
            _Goals = await GoalServices.GetGoals(true);
            if (_Goals.Any())
            {
                UpdateTaskItems();
            }
        }

        #endregion
    }
}
