using MaisQ1Dev.CashFlow.Reports.Domain.Companies;
using MaisQ1Dev.CashFlow.Reports.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaisQ1Dev.CashFlow.Reports.Infrastructure.Companies;

public class CompanyRepository : ICompanyRepository
{
    private readonly CashFlowReportDbContext _context;

    public CompanyRepository(CashFlowReportDbContext context)
        => _context = context;

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        => await _context.Companies.AnyAsync(c => c.Id == id, cancellationToken);

    public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Companies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
        => await _context.Companies.AddAsync(company, cancellationToken);

    public void Update(Company company)
        => _context.Companies.Update(company);
}