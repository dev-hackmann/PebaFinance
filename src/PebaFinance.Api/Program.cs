using PebaFinance.Infrastructure;
using PebaFinance.Api.Middleware;
using DotNetEnv;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using FluentMigrator.Runner;
using PebaFinance.Migrations;

namespace PebaFinance
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            Env.Load("../../.env");
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddApiServices();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ValidationFilter());
            });
            builder.Services.AddHealthChecks()
                .AddMySql(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    name: "mysql",
                    failureStatus: HealthStatus.Unhealthy,
                    timeout: TimeSpan.FromSeconds(5)
                );
            builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddMySql()
                .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
                .ScanIn(typeof(CreateInitialSchema).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("AllowFrontend");
            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}