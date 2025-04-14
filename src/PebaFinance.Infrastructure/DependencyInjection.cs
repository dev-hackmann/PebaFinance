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
            services.AddSingleton<IDbConnectionFactory>(provider =>
                new DbConnectionFactory(configuration));

            services.AddScoped<IIncomesRepository, IncomesRepository>();
            services.AddScoped<IExpensesRepository, ExpensesRepository>();
            services.AddScoped<ISummaryRepository, SummaryRepository>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(
                Assembly.GetExecutingAssembly(),
                Assembly.Load("PebaFinance.Application")
            );

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.Load("PebaFinance.Application"));

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Peba Finance API",
                    Version = "v1",
                    Description = "API for finance management with income and expenses tracking",
                    Contact = new OpenApiContact
                    {
                        Name = "Lucas Hackmann",
                        Email = "hackmann657@gmail.com"
                    }
                });
            });

            return services;
        }
    }
}