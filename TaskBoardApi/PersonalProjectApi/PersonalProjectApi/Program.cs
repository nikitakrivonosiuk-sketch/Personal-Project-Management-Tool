using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Data;
using PersonalProjectApi.Repositories.CardsRepositories;
using PersonalProjectApi.Repositories.ListRepositories;

var builder = WebApplication.CreateBuilder(args);

const string CORSOpenPolicy = "OpenCORSPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
      name: CORSOpenPolicy,
      builder => { builder
          .WithOrigins("*")
          .AllowAnyHeader()
          .AllowAnyMethod();
      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TaskBoardDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("TaskBoardConnectionString")));

builder.Services.AddScoped<ICardsRepository, SQLCardsRepository>();
builder.Services.AddScoped<IListsRepository, SQLListsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(CORSOpenPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
