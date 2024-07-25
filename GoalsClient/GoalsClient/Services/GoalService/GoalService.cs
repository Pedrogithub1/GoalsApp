using GoalsClient.Data.Entities;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoalsClient.Services.GoalService
{
    public class GoalService : IGoalService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _options;

        // Constructor
        public GoalService(HttpClient customHttpClientService, IJSRuntime jSRuntime)
        {
            _http = customHttpClientService;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public class GoalCreationResult
        {
            public bool Success { get; set; }
            public Goal? Goal { get; set; }
            public string? ErrorMessage { get; set; }
        }

        public class GoalUpdateResult
        {
            public bool Success { get; set; }
            public string? ErrorMessage { get; set; }
        }

        public async Task<GoalCreationResult> CreateGoal(Goal goal)
        {
            var response = await _http.PostAsJsonAsync("goals", goal);
            var result = new GoalCreationResult();

            if (response.IsSuccessStatusCode)
            {
                result.Goal = await response.Content.ReadFromJsonAsync<Goal>();
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

        public async Task<bool> DeleteGoal(int id)
        {
            var response = await _http.DeleteAsync($"goals/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;

        }

        public async Task<Goal> GetGoalById(int id, bool includeTaskItems = false)
        {
            var response = await _http.GetAsync($"goals/{id}?includeTaskItems={includeTaskItems}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var goal = JsonSerializer.Deserialize<Goal>(content, _options);

            return goal;
        }

        public async Task<List<Goal>> GetGoals(bool includeTaskItems = false)
        {
            var response = await _http.GetAsync($"goals?includeTaskItems={includeTaskItems}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }

            var goals = JsonSerializer.Deserialize<List<Goal>>(content, _options);

            return goals;
        }

        public async Task<GoalUpdateResult> UpdateGoal(Goal goal)
        {
            var response = await _http.PutAsJsonAsync($"goals/{goal.GoalId}", goal);
            var result = new GoalUpdateResult();

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
