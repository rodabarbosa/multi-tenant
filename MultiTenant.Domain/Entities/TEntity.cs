namespace MultiTenant.Domain.Entities;

public abstract class TEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted
    {
        get => DeletedAt.HasValue;
    }
}
