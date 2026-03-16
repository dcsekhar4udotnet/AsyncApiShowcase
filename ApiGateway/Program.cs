using ApiGateway.HttpClients;
using ApiGateway.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Typed HTTP clients keep service-specific configuration in one place.
builder.Services.AddHttpClient<IService1Client, Service1Client>(client =>
{
    var baseUrl = builder.Configuration["ServiceUrls:Service1"] ?? "http://localhost:5001";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<IService2Client, Service2Client>(client =>
{
    var baseUrl = builder.Configuration["ServiceUrls:Service2"] ?? "http://localhost:5002";
    client.BaseAddress = new Uri(baseUrl);
});

// Aggregates responses from the two backend services.
builder.Services.AddScoped<IAggregationService, AggregationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
