using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using BLogicCodeBase.Exceptions;
using BLogicCodeBase.Interfaces;
using Microsoft.Extensions.Configuration;
using MiniProfiler.Integrations;
using StackExchange.Profiling.Data;

namespace BLogicCodeBase.Implementations;

public class UnitOfWork(IConfiguration configuration) : IUnitOfWork
{
    private readonly string _connectionString = configuration.GetConnectionString("DBConnectionString")!
        ?? throw new CommonException($"Db connection string is not found!");
    DbConnection _connection = null;
    DbTransaction _transaction = null;
    Guid _id = Guid.Empty;

    private DbConnection Connection
    {
        get
        {
            if (CheckConnectionIsOpen()) return _connection;

            _id = Guid.NewGuid();
            _connection = new ProfiledDbConnection(new SqlConnection(_connectionString), CustomDbProfiler.Current);
            if (_connection.State != ConnectionState.Open && _connection.State != ConnectionState.Connecting)
            {
                _connection.Open();
            }

            return _connection;
        }
    }

    DbConnection IUnitOfWork.Connection => Connection;
    DbTransaction IUnitOfWork.Transaction
    {
        get { return _transaction; }
    }
    Guid IUnitOfWork.Id
    {
        get { return _id; }
    }

    public void Begin()
    {
        _transaction = Connection.BeginTransaction();
    }

    public DbCommand CreateCommand()
    {
        return Connection.CreateCommand();
    }
    
    public void Commit()
    {
        try
        {
            _transaction?.Commit();
        }
        catch (Exception)
        {
            _transaction.Rollback();
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        Dispose();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
        _transaction = null;
        _connection = null;
    }

    public async Task BeginAsync()
    {
        _transaction = await Connection.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _transaction?.CommitAsync();
        }
        catch (Exception)
        {
            await _transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        await _transaction?.RollbackAsync()!;
        await DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task<DbConnection> OpenConnectionAsync()
    {
        if (CheckConnectionIsOpen()) return _connection;

        _id = Guid.NewGuid();
        _connection = new ProfiledDbConnection(new SqlConnection(_connectionString), CustomDbProfiler.Current);
        if (_connection.State != ConnectionState.Open && _connection.State != ConnectionState.Connecting)
        {
            await _connection.OpenAsync();
        }

        return _connection;
    }

    private bool CheckConnectionIsOpen()
    {
        var profiledDbConnection = (ProfiledDbConnection)_connection;
        return profiledDbConnection?.WrappedConnection?.State == ConnectionState.Open || profiledDbConnection?.WrappedConnection?.State == ConnectionState.Connecting;
    }
}
