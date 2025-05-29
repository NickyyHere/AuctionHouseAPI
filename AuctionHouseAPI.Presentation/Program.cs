using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.MappingProfiles;
using AuctionHouseAPI.Application.Services;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Dapper;
using AuctionHouseAPI.Domain.Dapper.Repositories;
using AuctionHouseAPI.Domain.EFCore;
using AuctionHouseAPI.Domain.EFCore.Repositories;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("PGSQL_CONNECTION_STRING");

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
builder.Services.AddAuthorization();

string repositoryType = builder.Configuration["RepositoryType"]!;
switch (repositoryType)
{
    case "Dapper":
        Console.WriteLine("USING DAPPER");
        builder.Services.AddScoped<DapperContext>();
        builder.Services.AddScoped<IAuctionRepository, DapperAuctionRepository>();
        builder.Services.AddScoped<IBidRepository, DapperBidRepository>();
        builder.Services.AddScoped<ICategoryRepository, DapperCategoryRepository>();
        builder.Services.AddScoped<ITagRepository, DapperTagRepository>();
        builder.Services.AddScoped<IUserRepository, DapperUserRepository>();
        break;
    default:
        Console.WriteLine("USING EFCORE");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );
        builder.Services.AddScoped<IAuctionRepository, EFAuctionRepository>();
        builder.Services.AddScoped<IBidRepository, EFBidRepository>();
        builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
        builder.Services.AddScoped<ITagRepository, EFTagRepository>();
        builder.Services.AddScoped<IUserRepository, EFUserRepository>();
        break;
}

builder.Services.AddAutoMapper(typeof(AuctionMappingProfile));
builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));
builder.Services.AddAutoMapper(typeof(BidMappingProfile));

builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IBidService, BidService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();
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
