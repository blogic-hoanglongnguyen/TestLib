using System.Data.Common;

namespace BLogicCodeBase.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Guid Id { get; }
    DbConnection Connection { get; }
    DbTransaction Transaction { get; }
    void Begin();
    DbCommand CreateCommand();
    void Commit();
    void Rollback();
    Task BeginAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task<DbConnection> OpenConnectionAsync();
}
