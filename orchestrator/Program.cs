using application.Interfaces;
using application.Services;
using application.Validators;
using domain.Interfaces;
using FluentValidation;
using infrastructure.Adapters;
using infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Runtime.Versioning;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Set minimum log level
     .WriteTo.Console() // Log to console
    .WriteTo.File("logs/serilog-file.txt", rollingInterval: RollingInterval.Day) // Log to file, daily roll
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<TargetValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TemplateValidator>();
builder.Services.AddScoped<ITemplateDomain, TemplateAdapter>();
builder.Services.AddScoped<ITemplate, TemplateService>();
builder.Services.AddScoped<ITemplateLogDomain, TemplateAdapter>();
builder.Services.AddScoped<INatsManagement, NatsManagementAdapter>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEncryptAdapter, EncryptAdapter>();
builder.Services.AddScoped<IEncryptService, EncryptService>();
builder.Services.AddScoped<ITokenAdapter, TokenAdapter>();
builder.Services.AddScoped<IUserRepository<domain.Entities.User>, UserRepository>();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("issuer").Value,
            ValidAudience = builder.Configuration.GetSection("audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetSection("secretKey").Value))
        };

    });
builder.Host.UseSerilog();
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
