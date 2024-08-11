using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.IntegrationEvents.Companies;
using MassTransit;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.EventBus.Consumers;

public class CompanyUpdatedIntegrationEventConsumer : IConsumer<CompanyUpdatedIntegrationEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;

    public CompanyUpdatedIntegrationEventConsumer(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
    }

    public async Task Consume(ConsumeContext<CompanyUpdatedIntegrationEvent> context)
    {
        var company = await _companyRepository.GetByIdAsync(context.Message.CompanyId, default);
        if (company is null)
            return;

        company.Update(
            context.Message.Name,
            context.Message.Email);

        await _companyRepository.UpdateAsync(company, default);
        await _unitOfWork.SaveChangesAsync();
    }
}
