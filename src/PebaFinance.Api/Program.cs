using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PebaFinance.Application.Interfaces;
using PebaFinance.Infrastructure.Data;
using PebaFinance.Infrastructure.Data.Repositories;

namespace PebaFinance.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Finance API", Version = "v1" });
            });

            builder.Services.AddMediatR(
                Assembly.GetExecutingAssembly(), // Current assembly
                Assembly.Load("PebaFinance.Application") // Application assembly containing handlers
            );

            // Add FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add MySQL connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<FinanceDbContext>(options =>
                options.UseMySql(
                    connectionString, 
                    ServerVersion.AutoDetect(connectionString),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure()));

            // Register repositories
            builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
            builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

            var app = builder.Build();

            // Configure the HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}