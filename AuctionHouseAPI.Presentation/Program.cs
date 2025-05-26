using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.Mappers;
using AuctionHouseAPI.Application.Services;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories;
using AuctionHouseAPI.Domain.Repositories.interfaces;
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
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);
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

builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IMapper<UserDTO, CreateUserDTO, User>, UserMapper>();
builder.Services.AddSingleton<IMapper<AuctionDTO, CreateAuctionDTO, Auction>, AuctionMapper>();
builder.Services.AddSingleton<IMapper<CategoryDTO, CreateCategoryDTO, Category>, CategoryMapper>();
builder.Services.AddSingleton<IMapper<BidDTO, CreateBidDTO, Bid>, BidMapper>();

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
