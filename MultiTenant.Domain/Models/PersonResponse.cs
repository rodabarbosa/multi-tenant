namespace MultiTenant.Domain.Models;

public class PersonResponse
{
    public Guid PersonId { get; set; }
    public string Name { get; set; } = string.Empty;
}
