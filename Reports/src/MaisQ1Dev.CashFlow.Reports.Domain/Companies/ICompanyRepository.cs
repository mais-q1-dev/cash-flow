namespace MaisQ1Dev.CashFlow.Reports.Domain.Companies;

public interface ICompanyRepository
{
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);
    Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Company company, CancellationToken cancellationToken = default);
    Task UpdateAsync(Company company, CancellationToken cancellationToken = default);
}