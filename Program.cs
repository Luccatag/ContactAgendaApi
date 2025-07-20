using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ContactAgendaApi.Models;
using Microsoft.EntityFrameworkCore;
using ContactAgendaApi.Repositories;
using ContactAgendaApi.Interfaces; // Add this at the top
using ContactAgendaApi.Services; // at the top if not present

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// 🔹 Customized Swagger configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Contact Agenda API",
        Description = "A simple CRUD API for managing contacts",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your@email.com"
        }
    });

    // Optional: XML comments (requires enabling in .csproj)
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
// Configure Entity Framework Core to use SQLite as the database provider
// The connection string uses a local file-based database named "contacts.db"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=contacts.db"));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
// Register DapperContactRepository for DI
builder.Services.AddScoped<DapperContactRepository>();
// Register JsonContactRepository for DI (all contact CRUD will use contacts.json)
builder.Services.AddSingleton<IContactRepository>(provider =>
    new JsonContactRepository("contacts.json")
);
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapControllers();
app.MapRazorPages();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
