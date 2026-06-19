using Microsoft.AspNetCore.HttpLogging;
using EnablementFocusApi.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Bind strongly typed team focus settings from configuration
builder.Services.Configure<TeamFocusSettings>(
    builder.Configuration.GetSection(TeamFocusSettings.SectionName));

// Add HTTP logging to capture request/response details
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Authorization");
    logging.ResponseHeaders.Add("Content-Type");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Use HTTP logging middleware to capture all requests/responses
app.UseHttpLogging();

app.UseHttpsRedirection();

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("GetServerInfo endpoint called");
    var response = new
    {
        machineName = Environment.MachineName,
        timestamp = DateTime.UtcNow
    };
    logger.LogInformation("Returning server info: {MachineName}, {Timestamp}", response.machineName, response.timestamp);
    return Results.Ok(response);
})
.WithName("GetServerInfo")
.WithSummary("Returns information about the API host")
.WithDescription("Returns server information including machine name and current UTC timestamp.");



app.MapGet("/focusareas/{person}", (
    string person,
    IOptionsMonitor<TeamFocusSettings> teamFocusSettings,
    ILogger<Program> logger) =>
{
    logger.LogInformation("GetFocusAreas endpoint called with person: {Person}", person);    

    var teamMembers = teamFocusSettings.CurrentValue.TeamMembers;

    if (!teamMembers.TryGetValue(person, out var teamMember))
    {
        logger.LogWarning("No focus areas found for person: {Person}", person);
        return Results.NotFound(new
        {
            message = $"No focus areas found for {person}"
        });
    }

    var focusAreas = teamMember.FocusAreas;

    if (focusAreas is null)
    {
        logger.LogWarning("No focus areas found for person: {Person}", person);
        return Results.NotFound(new
        {
            message = $"No focus areas found for {person}"
        });
    }

    var response = new
    {
        name = person,
        focusAreas
    };
    logger.LogInformation("Found focus areas for {Person}: {FocusAreaCount} areas", person, focusAreas.Length);
    logger.LogDebug("Focus areas details: {@FocusAreas}", focusAreas);
    
    return Results.Ok(response);
})
.WithName("GetFocusAreas")
    .WithSummary("Returns focus areas for an Enablement team member")
    .WithDescription("Returns the current technical focus areas, expertise topics, and areas of responsibility for a specified Enablement team member.");


app.Run();

