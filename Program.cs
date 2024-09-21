using BlazorContactsApp.Components;
using BlazorContactsApp.Services;
using ContactsApi.Data;
using ContactsApi.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddControllers();
builder.Services.AddHttpClient<ContactService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5046"); // Replace with your actual base address
});

// Configure MongoDB context
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ContactDatabase"));
builder.Services.AddSingleton<ContactService>();

var logger = LoggerFactory
    .Create(loggingBuilder =>
    {
        loggingBuilder.AddConsole();
    })
    .CreateLogger("Program");

logger.LogInformation(
    "MongoDB Connection String: {ConnectionString}",
    builder.Configuration.GetValue<string>("ContactDatabase:ConnectionString")
);
logger.LogInformation(
    "MongoDB Database Name: {DatabaseName}",
    builder.Configuration.GetValue<string>("ContactDatabase:DatabaseName")
);

if (
    string.IsNullOrEmpty(builder.Configuration.GetValue<string>("ContactDatabase:ConnectionString"))
)
{
    throw new InvalidOperationException("MongoDB connection string is not configured.");
}

if (string.IsNullOrEmpty(builder.Configuration.GetValue<string>("ContactDatabase:DatabaseName")))
{
    throw new InvalidOperationException("MongoDB database name is not configured.");
}

var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");
logger.LogInformation("API Base URL: {ApiBaseUrl}", apiBaseUrl);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
