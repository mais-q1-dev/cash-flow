using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.UpdateTransaction;

public sealed class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;
    private readonly ITransactionRepository _transactionRepository;

    public UpdateTransactionHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        ITransactionRepository transactionRepository)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (transaction is null)
            return Result.NotFound(TransactionError.NotFound);

        if (transaction.CompanyId != request.CompanyId)
            return Result.NotFound(CompanyError.NotFound);

        transaction.Update(
            request.Type,
            request.Date,
            request.Amount,
            request.Description);

        _transactionRepository.Update(transaction);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
