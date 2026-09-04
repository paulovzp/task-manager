using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Api.Infrastructure;
using TaskManager.Application.Auth.Login;
using TaskManager.Application.Auth.Register;
using TaskManager.Application.Tasks.Create;
using TaskManager.Application.Tasks.Delete;
using TaskManager.Application.Tasks.Get;
using TaskManager.Application.Tasks.List;
using TaskManager.Application.Tasks.Update;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Identity;
using TaskManager.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<CreateTaskHandler>();
builder.Services.AddScoped<ListTasksHandler>();
builder.Services.AddScoped<GetTaskHandler>();
builder.Services.AddScoped<UpdateTaskHandler>();
builder.Services.AddScoped<DeleteTaskHandler>();

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
    ?? throw new InvalidOperationException("JWT configuration is required.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
        ClockSkew = TimeSpan.FromSeconds(30),
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(options => options.AddPolicy("Angular", policy => policy
    .WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:4200"])
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapOpenApi();
app.MapControllers();

if (builder.Configuration.GetValue<bool>("SeedData"))
{
    await DatabaseInitializer.InitializeAsync(app.Services);
}

await app.RunAsync();

/// <summary>Exposes the application entry point to integration tests.</summary>
public partial class Program;
