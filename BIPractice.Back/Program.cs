using Application;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

#region подключение к базе данных
string connectionString = builder.Configuration["MongoConnectionString"] ?? throw new ArgumentNullException("MongoConnectionString not found");

string databaseName = builder.Configuration["MongoDatabaseName"] ?? throw new ArgumentNullException("MongoDatabaseName not found");

builder.Services.AddSingleton<IMongoDatabase>(sp => new MongoClient(connectionString).GetDatabase(databaseName));

#endregion

builder.Services.AddScoped<IAppDbContext, BiContext>();

builder.Services.RegisterMediator();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.Run();