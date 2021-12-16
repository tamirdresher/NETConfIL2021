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

app.MapPost("/summary", (string summary) => summaries.Add(summary))
    .WithName("AddSummary")
    .WithTags("Summary")
    .Produces(200)
    .Produces(404); 

app.MapDelete("/summary/{id}", (int id, ILogger<WeatherForecast> logger) =>
    {
        if (id < 0 || id > summaries.Count - 1)
        {
            return Results.NotFound();
        }
        summaries.RemoveAt(id);
        logger.LogDebug("Removed summary {id}", id);
        return Results.Ok();
    })
    .WithName("DeleteSummary")
    .WithTags("Summary")
    .Produces(200)
    .Produces(404);



app.MapGet("/summary", (int id, ILogger<WeatherForecast> logger) =>
    {
        return summaries;
    })
    .ExcludeFromDescription();

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}