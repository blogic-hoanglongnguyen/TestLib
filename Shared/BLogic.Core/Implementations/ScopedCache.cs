using BLogicCodeBase.Interfaces;

namespace BLogicCodeBase.Implementations;

public sealed class ScopedCache : IScopedCache
{
    public Guid UserId { get; set; }
    public string UserType { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = [];
    public string Role { get; set; }
    public List<string> Permissions { get; set; } = [];
    public string ConnectionUuid { get; set; }
    public string RequestId{ get; set; }
    public string RequestCorrelationId { get; set; }
    public DateTimeOffset IssuedAt { get; set; }
}