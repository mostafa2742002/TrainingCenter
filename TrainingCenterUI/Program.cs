using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrainingCenterUI;
using TrainingCenterUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });5276
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5276/") });
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<StudentCoursesService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddTransient<AuthMessageHandler>();

builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5276/");
})
.AddHttpMessageHandler<AuthMessageHandler>();  // Append the auth token handler

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));

await builder.Build().RunAsync();
