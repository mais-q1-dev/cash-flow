using MaisQ1Dev.CashFlow.Reports.Application.Abstractions.Data;
using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.CashFlow.Reports.Domain.Transactions;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.Companies;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.Data;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.Data.Interceptors;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.Transactions;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Settings;
using MaisQ1Dev.Libs.IntegrationEvents.EventBus;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCashFlowInfra(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCashFlowOptions(configuration);
        services.AddCashFlowData(configuration);
        services.AddCashFlowEventBus(configuration);

        return services;
    }

    private static IServiceCollection AddCashFlowOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<MessageBusSetting>()
            .BindConfiguration(MessageBusSetting.Section)
            .ValidateOnStart();

        services.AddOptions<ConnectionStringsSetting>()
            .BindConfiguration(ConnectionStringsSetting.Section)
            .ValidateOnStart();

        return services;
    }

    private static IServiceCollection AddCashFlowData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();
        var connectionStringsSetting = serviceProvider.GetRequiredService<IOptions<ConnectionStringsSetting>>().Value;

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<CashFlowReportDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionStringsSetting.Database, options =>
            {
                options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                options.CommandTimeout(180);
            });
        });

        services.TryAddScoped<ICashFlowReportDbContext>(sp => sp.GetRequiredService<CashFlowReportDbContext>());

        services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CashFlowReportDbContext>());
        services.TryAddScoped<ICompanyRepository, CompanyRepository>();
        services.TryAddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }

    private static IServiceCollection AddCashFlowEventBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddDelayedMessageScheduler();

            config.AddConsumer<CompanyCreatedIntegrationEventConsumer>();
            config.AddConsumer<CompanyUpdatedIntegrationEventConsumer>();
            config.AddConsumer<TransactionCreatedIntegrationEventConsumer>();
            config.AddConsumer<TransactionUpdatedIntegrationEventConsumer>();

            config.AddEntityFrameworkOutbox<CashFlowReportDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });

            config.UsingRabbitMq((context, cfg) =>
            {
                var messageBusSetting = context.GetRequiredService<IOptions<MessageBusSetting>>().Value;

                cfg.ClearSerialization();
                cfg.UseRawJsonSerializer();

                cfg.Host(messageBusSetting.Host, messageBusSetting.VirtualHost, h =>
                {
                    h.Username(messageBusSetting.Username);
                    h.Password(messageBusSetting.Password);
                });

                cfg.UseDelayedMessageScheduler();
                cfg.ConfigureEndpoints(context);
            });
        });

        services.TryAddScoped<IEventBus, RabbitMQEventBus>();

        return services;
    }
}