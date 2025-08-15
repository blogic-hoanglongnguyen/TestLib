namespace BLogicCodeBase.Interfaces;

public interface IScopedCache
{
    Guid UserId { get; set; }
    string UserName { get; set; }
    string UserType { get; set; }
    string FullName { get; set; }
    string Email { get; set; }
    List<string> Roles { get; set; }
    List<string> Permissions { get; set; }
    string ConnectionUuid { get; set; }
    string RequestId { get; set; }
    string RequestCorrelationId { get; set; }
    DateTimeOffset IssuedAt { get; set; }
}