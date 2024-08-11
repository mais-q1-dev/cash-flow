using System.ComponentModel.DataAnnotations;

namespace MaisQ1Dev.Libs.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    [Timestamp]
    public byte[]? Version { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity() { }

    public void SetCreatedAt(DateTime utcNow) => CreatedAt = utcNow;
    public void SetUpdatedAt(DateTime utcNow) => UpdatedAt = utcNow;
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
