using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Logging;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Companies.CreateCompany;

public sealed class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;
    private readonly ILoggerMQ1Dev<CreateCompanyHandler> _logger;

    public CreateCompanyHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        ILoggerMQ1Dev<CreateCompanyHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var exists = await _companyRepository.Exists(request.CompanyId, default);
        if (exists)
        { 
            _logger.LogError("Company {CompanyId} already exists", request.CompanyId);
            return Result.UnprocessableEntity<Guid>(CompanyError.AlreadyExists);
        }

        var company = Company.Create(
            request.CompanyId,
            request.Name,
            request.Email);

        await _companyRepository.AddAsync(company, default);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Company {CompanyId} created", company.Id);
        return Result.Created(company.Id);

    }
}
