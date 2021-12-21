using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new List<string>
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Count)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/summary",(string summary)=>summaries.Add(summary));

app.MapDelete("/summary/{id}", (int id, ILogger<WeatherForecast> logger) => {
    summaries.RemoveAt(id);
    logger.LogDebug("Removed summary {id}", id);
});

app.MapGet("/{id}", ([FromRoute] int id,
                     [FromQuery(Name = "p")] int page,
                     [FromServices] HttpContext context,
                     [FromHeader(Name = "Content-Type")] string contentType) => { });


app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" }, () => "This is an options or head request ");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}