using Microsoft.EntityFrameworkCore;
using PersonalProjectApi.Controllers;
using PersonalProjectApi.Data;
using PersonalProjectApi.ExceptionHandler;
using PersonalProjectApi.Repositories.ActivityLogRepositories;
using PersonalProjectApi.Repositories.BoardRepository;
using PersonalProjectApi.Repositories.CardsRepositories;
using PersonalProjectApi.Repositories.Common;
using PersonalProjectApi.Repositories.ListRepositories;
using PersonalProjectApi.Services;

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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TaskBoardDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("TaskBoardConnectionString")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IBoardRepository, SQLBoardsRepository>();
builder.Services.AddScoped<ICardsRepository, SQLCardsRepository>();
builder.Services.AddScoped<IListsRepository, SQLListsRepository>();
builder.Services.AddScoped<IActivityLogRepository, SQLActivityLogRepository>();

builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<CardsService>();
builder.Services.AddScoped<ListService>();
builder.Services.AddScoped<LogsService>();

//ExceptionHandlers
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<TaskBoardDbContext>().Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors(CORSOpenPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
