using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.CashFlow.Transactions.Domain.Transactions;
using MaisQ1Dev.Libs.Domain;
using MaisQ1Dev.Libs.Domain.Database;
using MaisQ1Dev.Libs.Domain.Logging;
using MediatR;

namespace MaisQ1Dev.CashFlow.Transactions.Application.Transactions.CreateTransaction;

public sealed class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyRepository _companyRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILoggerMQ1Dev<CreateTransactionHandler> _logger;

    public CreateTransactionHandler(
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        ITransactionRepository transactionRepository,
        ILoggerMQ1Dev<CreateTransactionHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _companyRepository = companyRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var company = await _companyRepository.GetByIdAsync(request.CompanyId, cancellationToken);
        if (company is null)
        {
            _logger.LogError("Company {CompanyId} not found", request.CompanyId);
            return Result.NotFound<Guid>(CompanyError.NotFound);
        }

        var transaction = Transaction.Create(
            request.CompanyId,
            request.Type,
            request.Date,
            request.Amount,
            request.Description);

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Transaction {TransactionId} created for company {CompanyId}", 
            transaction.Id, 
            transaction.CompanyId);

        return Result.Created(transaction.Id);
    }
}
