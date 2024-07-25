using GoalsClient.Data.Entities;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoalsClient.Services.TaskItemService
{
    public class TaskItemsService : ITaskItemsService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _options;

        // Constructor
        public TaskItemsService(HttpClient customHttpClientService, IJSRuntime jSRuntime)
        {
            _http = customHttpClientService;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public class TaskItemCreationResult
        {
            public bool Success { get; set; }
            public TaskItem? TaskItem { get; set; }
            public string? ErrorMessage { get; set; }
        }

        public class TaskItemUpdateResult
        {
            public bool Success { get; set; }
            public string? ErrorMessage { get; set; }
        }

        public async Task<TaskItemCreationResult> CreateTaskItem(TaskItem taskItem)
        {
            var response = await _http.PostAsJsonAsync("taskItems", taskItem);
            var result = new TaskItemCreationResult();

            if (response.IsSuccessStatusCode)
            {
                result.TaskItem = await response.Content.ReadFromJsonAsync<TaskItem>();
                result.Success = true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                result.Success = false;

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    result.ErrorMessage = errorContent;
                }
                else
                {
                    result.ErrorMessage = $"Error: {response.StatusCode}, {errorContent}";
                }
            }

            return result;
        }

        public async Task<bool> DeleteTaskItem(int id)
        {
            var response = await _http.DeleteAsync($"taskItems/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;

        }

        public async Task<TaskItem> GetTaskItemById(int id)
        {
            var response = await _http.GetAsync($"taskItems/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var taskItem = JsonSerializer.Deserialize<TaskItem>(content, _options);

            return taskItem;
        }

        public async Task<List<TaskItem>> GetTaskItems()
        {
            var response = await _http.GetAsync($"taskItems");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var taskItems = JsonSerializer.Deserialize<List<TaskItem>>(content, _options);

            return taskItems;
        }

        public async Task<TaskItemUpdateResult> UpdateTaskItem(TaskItem taskItem)
        {
            var response = await _http.PutAsJsonAsync($"taskItems/{taskItem.TaskItemId}", taskItem);
            var result = new TaskItemUpdateResult();

            if (response.IsSuccessStatusCode)
            {
                result.Success = true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                result.Success = false;

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    result.ErrorMessage = errorContent;
                }
                else
                {
                    result.ErrorMessage = $"Error: {response.StatusCode}, {errorContent}";
                }
            }

            return result;
        }

    }
}
