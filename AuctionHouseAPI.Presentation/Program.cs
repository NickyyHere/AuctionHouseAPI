using AuctionHouseAPI.Migrations;
using AuctionHouseAPI.Presentation;
using AuctionHouseAPI.Shared.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("PGSQL_CONNECTION_STRING");
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
builder.Services.Configure<PgSqlDatabaseSettings>(options => options.ConnectionString = connectionString!);

builder.Services.AddHostedService<MigrationHostedService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("ROLE_ADMIN");
    });
});

string repositoryType = builder.Configuration["RepositoryType"]!;
switch (repositoryType)
{
    case "Dapper":
        builder.Services.AddDapperRepositories();
        break;
    default:
        Console.WriteLine("USING EFCORE");
        builder.Services.AddEFCoreRepositories(connectionString!);
        break;
}

builder.Services.AddMediatRHandlers();
builder.Services.AddMappers();
builder.Services.AddServices();

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!, name: "PostgreSQL")
    .AddMongoDb(sp => new MongoDB.Driver.MongoClient(mongoConnectionString), name: "MongoDB");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .MinimumLevel.Override("AuctionHouseAPI", LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.MongoDB(mongoConnectionString!, collectionName: "Logs")
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started at {Time}", DateTime.UtcNow);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
