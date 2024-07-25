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
        public List<Goal> _Goals { get; set; } = new();
        public List<TaskItem> _TaskItems { get; set; } = new();

        private Goal goalEdited = new();
        private TaskItem taskItemEdited = new();

        private bool isEditing = false;
        private bool isEditingTask = false;
        private string goalName = string.Empty;
        private string taskItemName = string.Empty;
        bool showGoalDialog = false;
        bool showTaskDialog = false;

        private DialogOptions dialogGoalOptions = new() { CloseOnEscapeKey = true, MaxWidth=MaxWidth.ExtraSmall, FullWidth = true, CloseButton = true };

        //Delete Modal
        private bool visibleDelete = false;
        public int deleteGoalId = 0;
        public int deleteTaskItemId = 0;
        public bool IsDeleteGoal = true;
        private DialogOptions dialogDeleteOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall, FullWidth = true, Position = DialogPosition.TopCenter, DisableBackdropClick = true, CloseButton = true };

        private TaskItem selectedTaskItem = null; 

        int goalSelectedId = -1;

        //Filters
        private string taskFilter = "";
        private DateTime? dateFilter;
        private string statusFilter = "";

        private IEnumerable<TaskItem> FilteredTaskItems => _TaskItems
            .Where(task => (string.IsNullOrEmpty(taskFilter) || task.Name.Contains(taskFilter, StringComparison.OrdinalIgnoreCase))
                          && (!dateFilter.HasValue || task.Date.Date == dateFilter.Value.Date)
                          && (string.IsNullOrEmpty(statusFilter) || task.Status.Contains(statusFilter, StringComparison.OrdinalIgnoreCase)));


        #endregion
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

        public void ShowGoalDialog(bool editing, Goal? goal = null)
        {
            isEditing = editing;
            if (isEditing)
            {
                goalEdited = JsonSerializer.Deserialize<Goal>(JsonSerializer.Serialize(goal));
                goalName = goal.Name;
            }
            else
            {
                goalName = string.Empty;
            }
            showGoalDialog = true;
        }

        public void ShowTaskItemDialog(bool editing)
        {

            isEditingTask = editing;
            if (isEditingTask)
            {
                taskItemEdited = JsonSerializer.Deserialize<TaskItem>(JsonSerializer.Serialize(selectedTaskItem));
                taskItemName = selectedTaskItem.Name;
            }
            else
            {
                taskItemName = string.Empty;
            }
            showTaskDialog = true;
        }

        private void CloseGoalDialog()
        {
            showGoalDialog = false;
        }

        private void CloseTaskDialog()
        {
            showGoalDialog = false;
        }


        //Create Goal
        private async Task CreateGoal()
        {

            Goal newGoal = new Goal();
            newGoal.Name = goalName;
            newGoal.TotalTasks = 0;
            newGoal.Date = DateTime.Now;

            Snackbar.Clear();

            var result = await GoalServices.CreateGoal(newGoal);

            if (result.Success)
            {
                Snackbar.Add($"Meta {goalName} creada!", Severity.Success);

                _Goals.Clear();
                _Goals = await GoalServices.GetGoals(true);
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

                _Goals.Clear();
                _Goals = await GoalServices.GetGoals(true);
                if (_Goals.Any())
                {
                    _TaskItems.Clear();
                    var selectedGoal = _Goals.FirstOrDefault(g => g.GoalId == goalSelectedId);
                    _TaskItems = selectedGoal?.TaskItems.ToList() ?? new List<TaskItem>();
                }
                showGoalDialog = false;
            }
            else
            {
                Snackbar.Add($"Error al actualizar meta: {result.ErrorMessage}", Severity.Error);
            }
        }

        private async Task DeleteGoal()
        {
            int deleteId = IsDeleteGoal ? deleteGoalId : deleteTaskItemId;
            bool response;

            if (IsDeleteGoal)
            {
                response = await GoalServices.DeleteGoal(deleteId);
            }
            else
            {
                response = await TaskItemsServices.DeleteTaskItem(deleteId);
                selectedTaskItem = null;
            }

            if (response)
            {
                string message = IsDeleteGoal ? "Meta eliminada!" : "Tarea eliminada!";
                Snackbar.Add(message, Severity.Success);

                _Goals.Clear();
                _Goals = await GoalServices.GetGoals(true);
                if (_Goals.Any())
                {
                    _TaskItems.Clear();
                    var selectedGoal = _Goals.FirstOrDefault(g => g.GoalId == goalSelectedId);
                    _TaskItems = selectedGoal?.TaskItems.ToList() ?? new List<TaskItem>();
                }
                showTaskDialog = false;
                StateHasChanged();
            }
            else
            {
                string errorMessage = IsDeleteGoal ? "Error al eliminar meta!" : "Error al eliminar tarea!";
                Snackbar.Add(errorMessage, Severity.Error);
            }

            visibleDelete = false;
        }


        //Delete Modal 

        private void OpenDeleteDialog(int deleteId, bool isGoal = true)
        {
            IsDeleteGoal = isGoal;
            if (isGoal)
            {
                deleteGoalId = deleteId;
            }
            else
            {
                deleteTaskItemId = deleteId;

            }
            visibleDelete = true;
        }
        void CloseDeleteModal() => visibleDelete = false;


        private void SelectGoal(int index)
        {
            goalSelectedId = index;
            _TaskItems.Clear();
            var selectedGoal = _Goals.FirstOrDefault(g => g.GoalId == goalSelectedId);
            _TaskItems = selectedGoal?.TaskItems.ToList() ?? new List<TaskItem>();
        }


        private void UpdateTaskItems()
        {
            var selectedGoal = _Goals.FirstOrDefault(g => g.GoalId == goalSelectedId);
            _TaskItems = selectedGoal?.TaskItems.ToList() ?? new List<TaskItem>();
        }

        private void SelectTaskItem(TaskItem taskItem)
        {
            if (selectedTaskItem == taskItem)
            {
                selectedTaskItem = null;
            }
            else
            {
                selectedTaskItem = taskItem;
            }
        }

        private bool IsTaskItemSelected => selectedTaskItem != null;


        //Create Goal
        private async Task CreateTaskItem()
        {

            TaskItem newTaskItem = new TaskItem();
            newTaskItem.Name = taskItemName;
            newTaskItem.Date = DateTime.Now;
            newTaskItem.Status = "Abierta";
            newTaskItem.GoalId = goalSelectedId;

            Snackbar.Clear();

            var result = await TaskItemsServices.CreateTaskItem(newTaskItem);

            if (result.Success)
            {
                Snackbar.Add($"Tarea {taskItemName} creada!", Severity.Success);

                _Goals.Clear();
                _Goals = await GoalServices.GetGoals(true);
                showTaskDialog = false;
                if (_Goals.Any())
                {
                    _TaskItems.Clear();
                    var selectedGoal = _Goals.FirstOrDefault(g => g.GoalId == goalSelectedId);
                    _TaskItems = selectedGoal?.TaskItems.ToList() ?? new List<TaskItem>();
                }
                StateHasChanged();
            }
            else
            {
                Snackbar.Add($"Error al crear tarea: {result.ErrorMessage}", Severity.Error);
            }
        }

        private async Task UpdateTaskItem(bool? isCompleted = false)
        {
            TaskItem updateTaskItem = new();
            if (isCompleted == true)
            {
                updateTaskItem = JsonSerializer.Deserialize<TaskItem>(JsonSerializer.Serialize(selectedTaskItem));
                updateTaskItem.Status = "Completada";

            }
            else
            {
                updateTaskItem = taskItemEdited;
                updateTaskItem.Name = taskItemName;

            }

            Snackbar.Clear();

            var result = await TaskItemsServices.UpdateTaskItem(updateTaskItem);

            if (result.Success)
            {
                Snackbar.Add($"Tarea {updateTaskItem.Name} actualizada!", Severity.Success);
                _Goals.Clear();
                _Goals = await GoalServices.GetGoals(true);
                if (_Goals.Any())
                {
                    _TaskItems.Clear();
                    var selectedGoal = _Goals.FirstOrDefault(g => g.GoalId == goalSelectedId);
                    _TaskItems = selectedGoal?.TaskItems.ToList() ?? new List<TaskItem>();
                }
                showTaskDialog = false;
                selectedTaskItem = null;
                StateHasChanged();
            }
            else
            {
                Snackbar.Add($"Error al actualizar tarea: {result.ErrorMessage}", Severity.Error);
            }
        }

    }
}