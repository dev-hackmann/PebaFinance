using System.Reflection;
using PebaFinance.Application.Interfaces;
using PebaFinance.Infrastructure.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PebaFinance.Infrastructure.Data.Repositories;

namespace PebaFinance.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FinanceDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null)
                )
            );
            
            // Register Repositories
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            
            return services;
        }
        
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add MediatR
            services.AddMediatR(
                Assembly.GetExecutingAssembly(), // Current assembly
                Assembly.Load("PebaFinance.Application") // Application assembly containing handlers
            );

            // Add FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.Load("PebaFinance.Application"));
            
            return services;
        }
        
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Add API services
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Finance API", 
                    Version = "v1",
                    Description = "API for finance management with income and expenses tracking",
                    Contact = new OpenApiContact
                    {
                        Name = "Your Name",
                        Email = "your.email@example.com"
                    }
                });
            });
            
            return services;
        }
    }
}