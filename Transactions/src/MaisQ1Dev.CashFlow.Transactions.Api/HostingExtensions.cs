using MaisQ1Dev.CashFlow.Transactions.Api.Endpoints;
using MaisQ1Dev.CashFlow.Transactions.Api.Middlewares;
using MaisQ1Dev.CashFlow.Transactions.Application;
using MaisQ1Dev.CashFlow.Transactions.Infrastructure;
using MaisQ1Dev.CashFlow.Transactions.Infrastructure.Data;
using MaisQ1Dev.Libs.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Serilog;

namespace MaisQ1Dev.CashFlow.Transactions.Api;

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
                tags: ["db", "postgres"]);

        builder.Services.TryAddTransient<LoggingContextMiddleware>();

        return builder;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseMiddleware<LoggingContextMiddleware>();

        app.UseCors("cors");
        app.UseSerilogRequestLogging();

        app.MapHealthChecks("/health");
        app.MapCompaniesEndpoints();
        app.MapTransactionsEndpoints();

        return app;
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowTransactionDbContext>();

        dbContext.Database.Migrate();
    }
}