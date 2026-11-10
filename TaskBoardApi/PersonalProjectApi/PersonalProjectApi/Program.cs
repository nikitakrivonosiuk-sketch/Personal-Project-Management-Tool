using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalProjectApi.Data;
using PersonalProjectApi.ExceptionHandler;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Repositories.ActivityLogRepositories;
using PersonalProjectApi.Repositories.BoardMembersRepositories;
using PersonalProjectApi.Repositories.BoardRepository;
using PersonalProjectApi.Repositories.CardsRepositories;
using PersonalProjectApi.Repositories.Common;
using PersonalProjectApi.Repositories.ListRepositories;
using PersonalProjectApi.Services;
using PersonalProjectApi.Services.Authorization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUri = builder.Configuration["KeyVault:Uri"];
if (!string.IsNullOrEmpty(keyVaultUri))
{
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
}

const string CORSOpenPolicy = "OpenCORSPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
      name: CORSOpenPolicy,
      builder => { builder
          .WithOrigins("http://localhost:4200")
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
options.UseSqlServer(
    builder.Configuration.GetConnectionString("TaskBoardConnectionString"),
    sql => sql.EnableRetryOnFailure(
        maxRetryCount: 8,
        maxRetryDelay: TimeSpan.FromSeconds(15),
        errorNumbersToAdd: null)));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IBoardRepository, SQLBoardsRepository>();
builder.Services.AddScoped<ICardsRepository, SQLCardsRepository>();
builder.Services.AddScoped<IListsRepository, SQLListsRepository>();
builder.Services.AddScoped<IActivityLogRepository, SQLActivityLogRepository>();
builder.Services.AddScoped<IBoardMembersRepository, SQLBoardMemberRepository>();

builder.Services.AddScoped<BoardService>();
builder.Services.AddScoped<BoardMemberService>();
builder.Services.AddScoped<CardsService>();
builder.Services.AddScoped<ListService>();
builder.Services.AddScoped<LogsService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<BoardAuthorizationService>();
builder.Services.AddAuthorization();

//ExceptionHandlers
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequiredLength = 10;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
})
    .AddEntityFrameworkStores<TaskBoardDbContext>()
    .AddSignInManager();

var signingKey = builder.Configuration["Jwt:SigningKey"]
    ?? throw new InvalidOperationException("JWT signing key is missing."); ;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),

            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,
        };
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
