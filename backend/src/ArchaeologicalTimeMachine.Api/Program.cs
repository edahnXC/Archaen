using System.Text.Json.Serialization;
using ArchaeologicalTimeMachine.Infrastructure.Persistence;
using ArchaeologicalTimeMachine.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers with JSON formatting options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// 2. Configure Entity Framework Core with SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=archaeology.db";
builder.Services.AddDbContext<ArchaeologyDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

// 3. Configure CORS to allow Angular frontend development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 4. OpenAPI / Swagger setup
builder.Services.AddOpenApi();

var app = builder.Build();

// 5. Database Initialization & Seeding on Startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ArchaeologyDbContext>();
        await DatabaseSeeder.SeedAsync(context);
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Archaeological Time Machine database verified and seeded successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the Archaeological Time Machine database.");
    }
}

// 6. HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

// Friendly health & diagnostic root endpoint
app.MapGet("/", async (ArchaeologyDbContext context) =>
{
    int siteCount = await context.Sites.CountAsync();
    int civCount = await context.Civilizations.CountAsync();
    int artefactCount = await context.Artefacts.CountAsync();

    return Results.Ok(new
    {
        Project = "Archaeological Time Machine Web API",
        Status = "Operational",
        Environment = app.Environment.EnvironmentName,
        Stats = new
        {
            TotalSites = siteCount,
            TotalCivilizations = civCount,
            TotalArtefacts = artefactCount
        },
        Endpoints = new[]
        {
            "/api/sites",
            "/api/sites/{id}",
            "/api/sites/{id}/nearby",
            "/api/sites/{id}/contemporaneous",
            "/api/sites/{id}/stratigraphy",
            "/api/civilizations",
            "/api/periods",
            "/api/artefacts",
            "/api/sites/compare?site1Id=1&site2Id=2"
        }
    });
});

app.MapControllers();

app.Run();
