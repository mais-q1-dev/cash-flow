using HealthChecks.UI.Client;
using MaisQ1Dev.CashFlow.Reports.Api.Endpoints;
using MaisQ1Dev.CashFlow.Reports.Api.Middlewares;
using MaisQ1Dev.CashFlow.Reports.Application;
using MaisQ1Dev.CashFlow.Reports.Infrastructure;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.Data;
using MaisQ1Dev.Libs.Domain.Settings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Filters;

namespace MaisQ1Dev.CashFlow.Reports.Api;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(
        this WebApplicationBuilder builder)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.UseInlineDefinitionsForEnums();
            options.EnableAnnotations();
        });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCashFlowApplication(builder.Configuration);
        builder.Services.AddCashFlowInfra(builder.Configuration);

        builder.Services.AddCors(
            options => options.AddPolicy(
                "cors",
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            ));

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        var serviceProvider = builder.Services.BuildServiceProvider();
        var connectionStringsSetting = serviceProvider.GetRequiredService<IOptions<ConnectionStringsSetting>>().Value;
        var rabbitMqSettings = serviceProvider.GetRequiredService<IOptions<MessageBusSetting>>().Value;

        builder.Services.AddHealthChecks()
            .AddCheck("API Health Check", () => HealthCheckResult.Healthy("Reports Api is running"))
            .AddRabbitMQ(rabbitMqSettings.ConnectionString,
                name: "RabbitMQ",
                tags: ["rabbitmq"])
            .AddNpgSql(connectionStringsSetting.Database,
                name: "PostgreSQL",
                tags: ["postgres"]);

        builder.Services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(5);
            options.MaximumHistoryEntriesPerEndpoint(10);
            options.AddHealthCheckEndpoint("Reports Api Health Checks", "/health");
        })
        .AddInMemoryStorage();

        builder.Services.TryAddTransient<CorrelationMiddleware>();

        return builder;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseMiddleware<CorrelationMiddleware>();

        app.UseCors("cors");
        app.UseSerilogRequestLogging();

        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            Predicate = p => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecksUI(options => { options.UIPath = "/dashboard"; });

        app.MapCompaniesEndpoints();
        app.MapTransactionsEndpoints();

        return app;
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowReportDbContext>();

        dbContext.Database.Migrate();
    }
}