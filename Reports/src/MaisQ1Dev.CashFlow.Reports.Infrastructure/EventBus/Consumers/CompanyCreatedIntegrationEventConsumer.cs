using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MassTransit;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public class CompanyCreatedIntegrationEventConsumer : IConsumer<CompanyCreatedIntegrationEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;

    public CompanyCreatedIntegrationEventConsumer(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
    }

    public async Task Consume(ConsumeContext<CompanyCreatedIntegrationEvent> context)
    {
        var exists = await _companyRepository.Exists(context.Message.CompanyId, default);
        if (exists)
            return;

        var company = Company.Create(
            context.Message.CompanyId,
            context.Message.Name,
            context.Message.Email);

        await _companyRepository.AddAsync(company, default);
        await _unitOfWork.SaveChangesAsync();
    }
}

