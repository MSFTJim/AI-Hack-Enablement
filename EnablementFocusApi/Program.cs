using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

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



app.MapGet("/focusareas/{person}", (string person, ILogger<Program> logger) =>
{
    logger.LogInformation("GetFocusAreas endpoint called with person: {Person}", person);    


    var data = new Dictionary<string, string[]>
    {
        ["jim"] = new[]
        {
            "Azure AI Foundry",
            "AI Apps & Agents",
            "Technical Enablement",
            "Fabric IQ",
            "Foundry IQ"
        },

        ["reni"] = new[]
        {
            "Technical Readiness",
            "Fabric",
            "Fabric IQ"
        },

        ["chris"] = new[]
        {
            "GitHub Copilot",
            "Foundry IQ"
            
        },

        ["jc"] = new[]
        {
            "SQL Server",
            "Database Migrations",
            "Fabric"
        }
    };

    if (!data.TryGetValue(person.ToLower(), out var focusAreas))
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

