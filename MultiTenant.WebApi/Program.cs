using MultiTenant.WebApi.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddResponseCompression();
builder.Services.AddCors();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement;
        options.JsonSerializerOptions.WriteIndented = false;
    });

builder.AddJwtService()
    .ConfigureSwagger()
    .AddDatabase()
    .AddServices();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseSwaggerApp();

app.UseHttpsRedirection()
    .UseResponseCompression()
    .UseAuthorization()
    .UseCors(options => options
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());

app.MapControllers();

app.Run();
