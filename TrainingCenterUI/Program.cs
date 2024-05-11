using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using TrainingCenterUI;
using TrainingCenterUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5276/") });
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<StudentCoursesService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<HttpClient>(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    var jsRuntime = sp.GetRequiredService<IJSRuntime>();
    var authToken = jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken").Result;

    var httpClient = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5276")
    };

    if (!string.IsNullOrWhiteSpace(authToken))
    {
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
    }

    return httpClient;
});


await builder.Build().RunAsync();
