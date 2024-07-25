global using GoalsClient.Services.GoalService;
global using GoalsClient.Services.TaskItemService;
using GoalsClient;
using GoalsClient.Services.GoalService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Services
builder.Services.AddMudServices();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<ITaskItemsService, TaskItemsService>();

// Connection to API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7236/api/"), Timeout = TimeSpan.FromMinutes(15) });

await builder.Build().RunAsync();
