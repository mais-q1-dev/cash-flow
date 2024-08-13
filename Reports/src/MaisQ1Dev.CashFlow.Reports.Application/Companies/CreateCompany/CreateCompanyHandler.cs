using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MediatR;

namespace MaisQ1Dev.CashFlow.Reports.Application.Companies.CreateCompany;

public sealed class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;

    public CreateCompanyHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
    }

    public async Task<Result<Guid>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var exists = await _companyRepository.Exists(request.CompanyId, default);
        if (exists)
            return Result.UnprocessableEntity<Guid>(CompanyError.AlreadyExists);

        var company = Company.Create(
            request.CompanyId,
            request.Name,
            request.Email);

        await _companyRepository.AddAsync(company, default);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(company.Id);

    }
}
