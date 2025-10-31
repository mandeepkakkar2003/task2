using System.Text;
using Api.Middleware;
using Application.Abstractions;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Config
var conn = builder.Configuration.GetConnectionString("Default") ?? "Data Source=app.db";
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite(conn));

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    // defaults are fine
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for frontend origin
var frontOrigin = builder.Configuration["FrontendOrigin"] ?? "http://localhost:5173";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("front", p => p.WithOrigins(frontOrigin)
        .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

// Auth
var key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt__Key missing");
var issuer = builder.Configuration["Jwt:Issuer"] ?? "minimal-pm";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new()
        {
            ValidateIssuer = true, ValidIssuer = issuer,
            ValidateAudience = true, ValidAudience = issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();

// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ISchedulerService, SchedulerService>();
builder.Services.AddSingleton<PasswordHasher>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("front");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
