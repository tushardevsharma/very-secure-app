var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

// Read the secrets from files in /etc/secrets directory
var v2Acc = File.ReadAllText("/etc/secrets/v2-acc-file").Trim();
var v3Acc = File.ReadAllText("/etc/secrets/v3-acc-file").Trim();

// You can now use these values in the configuration or elsewhere in the app
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    { "V2_Acc", v2Acc },
    { "V3_Acc", v3Acc },
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
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

app.MapGet("/secrets", () =>
    {
        var v2Acc = builder.Configuration["V2_Acc"];
        var v3Acc = builder.Configuration["V3_Acc"];

        return new { V2Acc = v2Acc, V3Acc = v3Acc, };
    })
    .WithName("Getk8Secret")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}