using MaisQ1Dev.CashFlow.Transactions.Domain.Companies;
using MaisQ1Dev.CashFlow.Transactions.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Transactions.Infrastructure.Companies;

public class CompanyRepository : ICompanyRepository
{
    private readonly CashFlowTransactionDbContext _context;

    public CompanyRepository(CashFlowTransactionDbContext context)
        => _context = context;

    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Companies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
        => await _context.Companies.AddAsync(company, cancellationToken);

    public async Task UpdateAsync(Company company, CancellationToken cancellationToken = default)
        => await Task.Run(() => _context.Companies.Update(company), cancellationToken);
}