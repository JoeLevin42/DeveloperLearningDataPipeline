using SurveyApi.Models;
using SurveyApi.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// MongoDB configuration

var mongoConnection =
    Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")
    ?? builder.Configuration["SurveyDatabase:ConnectionString"];

var mongoDatabase =
    Environment.GetEnvironmentVariable("MONGO_DATABASE")
    ?? builder.Configuration["SurveyDatabase:DatabaseName"];


// Create SurveyDatabase configuration

builder.Services.Configure<SurveyDatabaseSettings>(options =>
{
    options.ConnectionString = mongoConnection;
    options.DatabaseName = mongoDatabase;
});


builder.Services.AddSingleton<SurveyService>();


var app = builder.Build();


// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();