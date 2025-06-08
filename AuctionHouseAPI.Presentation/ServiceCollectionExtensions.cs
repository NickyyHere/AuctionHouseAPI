using AuctionHouseAPI.Domain.EFCore.Repositories;
using AuctionHouseAPI.Domain.EFCore;
using AuctionHouseAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using AuctionHouseAPI.Domain.Dapper.Repositories;
using AuctionHouseAPI.Domain.Dapper;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Application.Services;
using AuctionHouseAPI.Application.MappingProfiles;
using AuctionHouseAPI.Application.CQRS;
using FluentValidation;
using MediatR;
using AuctionHouseAPI.Application.CQRS.Pipelines;

namespace AuctionHouseAPI.Presentation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFCoreRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
            services.AddScoped<IAuctionRepository, EFAuctionRepository>();
            services.AddScoped<IBidRepository, EFBidRepository>();
            services.AddScoped<ICategoryRepository, EFCategoryRepository>();
            services.AddScoped<ITagRepository, EFTagRepository>();
            services.AddScoped<IUserRepository, EFUserRepository>();
            return services;
        }
        public static IServiceCollection AddDapperRepositories(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();
            services.AddScoped<IAuctionRepository, DapperAuctionRepository>();
            services.AddScoped<IBidRepository, DapperBidRepository>();
            services.AddScoped<ICategoryRepository, DapperCategoryRepository>();
            services.AddScoped<ITagRepository, DapperTagRepository>();
            services.AddScoped<IUserRepository, DapperUserRepository>();
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IBidService, BidService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuctionMappingProfile));
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(CategoryMappingProfile));
            services.AddAutoMapper(typeof(BidMappingProfile));
            return services;
        }
        public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CQRSAssemblyReference).Assembly));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(CQRSAssemblyReference).Assembly);
            return services;
        }
    }
}
