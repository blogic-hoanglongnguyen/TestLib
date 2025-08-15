using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using BLogicCodeBase.Entities;
using BLogicCodeBase.Exceptions;
using BLogicCodeBase.Interfaces;
using BLogicCodeBase.Visitors;
using Dapper;
using Dapper.Contrib.Extensions;
using TableAttribute = Dapper.Contrib.Extensions.TableAttribute;

namespace BLogicCodeBase.Implementations;

public abstract class BaseRepository<TEntity>(IUnitOfWork unitOfWork, IScopedCache scopedCache) : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IScopedCache _scopedCache = scopedCache;
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    private bool _isAllowedAuditLog = true;

    #region Audit
    public virtual void BeforeCreate(TEntity entity)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.CreatedBy = _scopedCache.UserId;
    }

    public virtual void BeforeUpdate(TEntity entity)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedBy = _scopedCache.UserId;
    }

    public virtual void BeforeDelete(TEntity entity)
    {
        
    }

    public void SetToEnableAuditTracking(bool isAllowedAuditLog)
    {
        _isAllowedAuditLog = isAllowedAuditLog;
    }
    #endregion
    
    #region Sync
    public virtual TEntity GetById(Guid id)
    {
        string tableName = GetTableName();
        var query = @$"SELECT TOP 1 * FROM {tableName} WHERE Id = @Id";
        var entity = _unitOfWork.Connection.QueryFirstOrDefault<TEntity>(query, new { Id = id }, transaction:  _unitOfWork.Transaction);
        return entity;
    }
    
    public virtual TEntity GetFirstOrDefaultByCondition(Expression<Func<TEntity, bool>> predicate = null)
    {
        var (sql, parameters) = GetQueryStringAndParasFromPredicate(predicate);
        var result = _unitOfWork.Connection.QueryFirstOrDefault(sql, parameters);
        return result;
    }
    
    public virtual List<TEntity> GetListByCondition(Expression<Func<TEntity, bool>> predicate = null)
    {
        var (sql, parameters) = GetQueryStringAndParasFromPredicate(predicate);
        var result = _unitOfWork.Connection.Query<TEntity>(sql, parameters);
        return result.ToList();
    }

    public virtual TEntity Create(TEntity entity)
    {
        AddAuditForEntityIfEnabled(BeforeCreate, entity);
        _unitOfWork.Connection.Insert(entity, transaction: _unitOfWork.Transaction);
        return entity;
    }

    public virtual TEntity Update(TEntity entity)
    {
        AddAuditForEntityIfEnabled(BeforeUpdate, entity);
        _unitOfWork.Connection.Update(entity, transaction: _unitOfWork.Transaction);
        return entity;
    }

    public virtual Guid Delete(TEntity entity)
    {
        AddAuditForEntityIfEnabled(BeforeDelete, entity);
        _unitOfWork.Connection.Delete(entity, transaction: _unitOfWork.Transaction);
        return entity.Id;
    }
    
    public virtual int DeleteById(Guid id)
    {
        string tableName = GetTableName();
        var query = @$"DELET FROM {tableName} WHERE Id = @Id"; 
        var affectedRow = _unitOfWork.Connection.Execute(query, new { Id = id } , transaction: _unitOfWork.Transaction);
        return affectedRow;
    }

    public virtual List<TEntity> Create(List<TEntity> entities)
    {
        AddAuditForEntitiesIfEnabled(BeforeCreate, entities);
        _unitOfWork.Connection.Insert(entities, transaction: _unitOfWork.Transaction);
        return entities;
    }

    public virtual  List<TEntity> Update(List<TEntity> entities)
    {
        AddAuditForEntitiesIfEnabled(BeforeUpdate, entities);
        _unitOfWork.Connection.Update(entities, transaction: _unitOfWork.Transaction);
        return entities;
    }

    public virtual  List<Guid> Delete(List<TEntity> entities)
    {
        AddAuditForEntitiesIfEnabled(BeforeDelete, entities);
        _unitOfWork.Connection.Delete(entities, transaction: _unitOfWork.Transaction);
        return entities.Select(x => x.Id).ToList();
    }
    #endregion

    #region Async
    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        string tableName = GetTableName();
        var query = @$"SELECT TOP 1 * FROM {tableName} WHERE Id = @Id";
        var entity = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<TEntity>(query, new { Id = id }, _unitOfWork.Transaction);
        return entity;
    }

    public virtual async Task<TEntity> GetFirstOrDefaultByConditionAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var (sql, parameters) = GetQueryStringAndParasFromPredicate(predicate);
        var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);
        return result;
    }
    
    public virtual async Task<List<TEntity>> GetListByConditionAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        var (sql, parameters) = GetQueryStringAndParasFromPredicate(predicate);
        var result = await _unitOfWork.Connection.QueryAsync<TEntity>(sql, parameters);
        return result.ToList();
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        AddAuditForEntityIfEnabled(BeforeCreate, entity);
        await _unitOfWork.Connection.InsertAsync(entity, transaction: _unitOfWork.Transaction);
        return entity;
    }

    public virtual  async Task<TEntity> UpdateAsync(TEntity entity)
    {
        AddAuditForEntityIfEnabled(BeforeUpdate, entity);
        await _unitOfWork.Connection.UpdateAsync(entity, transaction: _unitOfWork.Transaction);
        return entity;
    }
 
    public virtual async Task<Guid> DeleteAsync(TEntity entity)
    {
        AddAuditForEntityIfEnabled(BeforeDelete, entity);
        await _unitOfWork.Connection.DeleteAsync(entity, transaction: _unitOfWork.Transaction);
        return entity.Id;
    }

    public virtual async Task<int> DeleteByIdAsync(Guid id)
    {
        string tableName = GetTableName();
        var query = @$"DELET FROM {tableName} WHERE Id = @Id"; 
        var affectedRow = await _unitOfWork.Connection.ExecuteAsync(query, new { Id = id } , transaction: _unitOfWork.Transaction);
        return affectedRow;
    }
    
    public virtual  async Task<List<TEntity>> CreateAsync(List<TEntity> entities)
    {
        AddAuditForEntitiesIfEnabled(BeforeCreate, entities);
        await _unitOfWork.Connection.InsertAsync(entities, transaction: _unitOfWork.Transaction);
        return entities;
    }

    public virtual  async Task<List<TEntity>> UpdateAsync(List<TEntity> entities)
    {
        AddAuditForEntitiesIfEnabled(BeforeUpdate, entities);
        await _unitOfWork.Connection.UpdateAsync(entities, transaction: _unitOfWork.Transaction);
        return entities;
    }
 
    public virtual  async Task<List<Guid>> DeleteAsync(List<TEntity> entities)
    {
        AddAuditForEntitiesIfEnabled(BeforeDelete, entities);
        await _unitOfWork.Connection.DeleteAsync(entities, transaction: _unitOfWork.Transaction);
        return entities.Select(x => x.Id).ToList();
    }
    #endregion

    #region Private function
    private string GetTableName()
    {
        var type = typeof(TEntity);
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        return tableAttr?.Name
            ?? throw new CommonException($"No table attribute configured for entity {type.Name}", (int)HttpStatusCode.InternalServerError, _ignoreLog: true);
    }    
    private void AddAuditForEntityIfEnabled(Action<TEntity> auditAction, TEntity entity)
    {
        if (_isAllowedAuditLog && auditAction != null)
        {
            auditAction(entity);
        }
    }
    
    private void AddAuditForEntitiesIfEnabled(Action<TEntity> auditAction, List<TEntity> entities)
    {
        if (_isAllowedAuditLog)
        {
            foreach (var entity in entities)
            {
                auditAction(entity);
            }
        }
    }

    private (string, DynamicParameters) GetQueryStringAndParasFromPredicate(Expression<Func<TEntity, bool>> predicate = null)
    {
        string tableName = GetTableName();
        if (predicate is null)
            return ($"SELECT * FROM {tableName}", new DynamicParameters());
        
        var visitor = new SqlExpressionVisitor();
       
        var (whereSql, parameters) = visitor.Translate(predicate.Body);
    
        var sql = $"SELECT * FROM {tableName} WHERE {whereSql}";
        return (sql, parameters);
    }
    #endregion
}
