using System.Linq.Expressions;
using BLogicCodeBase.Entities;

namespace BLogicCodeBase.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    void BeforeCreate(TEntity entity);
    void BeforeUpdate(TEntity entity);
    void BeforeDelete(TEntity entity);
    void SetToEnableAuditTracking(bool isAllowedAuditLog);
    
    Task<TEntity> GetByIdAsync(Guid Id);
    Task<TEntity> GetFirstOrDefaultByConditionAsync(Expression<Func<TEntity, bool>> predicate = null);
    Task<List<TEntity>> GetListByConditionAsync(Expression<Func<TEntity, bool>> predicate = null);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<Guid> DeleteAsync(TEntity entity);
    Task<int> DeleteByIdAsync(Guid id);
    Task<List<TEntity>> CreateAsync(List<TEntity> entities);
    Task<List<TEntity>> UpdateAsync(List<TEntity> entities);
    Task<List<Guid>> DeleteAsync(List<TEntity> entities);

    TEntity GetById(Guid Id);
    TEntity GetFirstOrDefaultByCondition(Expression<Func<TEntity, bool>> predicate = null);
    List<TEntity> GetListByCondition(Expression<Func<TEntity, bool>> predicate = null);
    TEntity Create(TEntity entity);
    TEntity Update(TEntity entity);
    Guid Delete(TEntity entity);
    int DeleteById(Guid id);
    List<TEntity> Create(List<TEntity> entities);
    List<TEntity> Update(List<TEntity> entities);
    List<Guid> Delete(List<TEntity> entities);
}
