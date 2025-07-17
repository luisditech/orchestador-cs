namespace TropicFeel.Domain.Entities;

public class ProductMapping : BaseAuditableEntity
{
    public required string JlpSku { get; set; }
    public required string SprintSku { get; set; }
    public required string NetSuiteSku { get; set; }
}
