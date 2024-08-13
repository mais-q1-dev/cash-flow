using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Companies.UpdateCompany;

public sealed class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;

    public UpdateCompanyHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
    }

    public async Task<Result> Handle(
        UpdateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (company is null)
            return Result.NotFound(CompanyError.NotFound);

        company.Update(request.Name, request.Email);

        _companyRepository.Update(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
