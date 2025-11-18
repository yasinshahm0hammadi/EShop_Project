using EShop.Domain.Entities.Common;

namespace EShop.Domain.Repository.Interface;

public interface IGenericRepository<TEntity> : IAsyncDisposable where TEntity : BaseEntity
{
    IQueryable<TEntity> GetQuery();
    Task AddEntity(TEntity entity, string? creatorName);
    Task AddRangeEntity(List<TEntity> entities, string? creatorName);
    Task<TEntity> GetEntityById(long entityId);
    void EditEntity(TEntity entity, string? modifierName);
    void UnpublishEntity(TEntity entity, string? modifierName);
    Task UnpublishEntityById(long entityId, string? modifierName);
    void DeleteEntity(TEntity entity);
    void DeleteEntities(List<TEntity> entityIds);
    Task SaveChanges();
}