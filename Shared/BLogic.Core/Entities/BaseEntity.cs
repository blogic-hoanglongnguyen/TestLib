using Dapper.Contrib.Extensions;

namespace BLogicCodeBase.Entities;

public abstract class BaseEntity
{
    [ExplicitKey]
    public Guid Id { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}