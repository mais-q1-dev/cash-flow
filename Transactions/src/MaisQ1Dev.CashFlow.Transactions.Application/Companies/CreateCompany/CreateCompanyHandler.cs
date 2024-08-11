using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.CreateCompany;

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

    public async Task<Result<Guid>> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var company = Company.Create(request.Name, request.Email);

        await _companyRepository.AddAsync(company, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Created(company.Id);
    }
}