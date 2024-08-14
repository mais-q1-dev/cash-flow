using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Logging;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Companies.UpdateCompany;

public sealed class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;
    private readonly ILoggerMQ1Dev<UpdateCompanyHandler> _logger;

    public UpdateCompanyHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        ILoggerMQ1Dev<UpdateCompanyHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.CompanyId, default);
        if (company is null)
        {
            _logger.LogError("Company {CompanyId} not found", request.CompanyId);
            return Result.NotFound(CompanyError.NotFound);
        }

        company.Update(
            request.Name,
            request.Email);

        _companyRepository.Update(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Company {CompanyId} updated", company.Id);
        return Result.NoContent();
    }
}
