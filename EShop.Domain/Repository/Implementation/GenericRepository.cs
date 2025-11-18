using EShop.Domain.Context;
using EShop.Domain.Entities.Common;
using EShop.Domain.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EShop.Domain.Repository.Implementation;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    #region Fields

    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    #endregion

    #region Conctructor

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    #endregion

    public IQueryable<TEntity> GetQuery()
    {
        return _dbSet.AsQueryable();
    }

    public async Task AddEntity(TEntity entity, string? creatorName)
    {
        entity.CreatedAt = DateTime.Now;
        entity.LastModifiedAt = DateTime.Now;
        entity.CreatedBy = creatorName;
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeEntity(List<TEntity> entities, string? creatorName)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.Now;
            entity.LastModifiedAt = DateTime.Now;
            entity.CreatedBy = creatorName;
            await AddEntity(entity, creatorName);
        }
    }

    public async Task<TEntity> GetEntityById(long entityId)
    {
        return await _dbSet.SingleOrDefaultAsync(x => x.Id == entityId);
    }

    public void EditEntity(TEntity entity, string? modifierName)
    {
        entity.LastModifiedAt = DateTime.Now;
        entity.Modifiedby = modifierName;
        _dbSet.Update(entity);
    }

    public void UnpublishEntity(TEntity entity, string? modifierName)
    {
        entity.IsPublished = true;
        EditEntity(entity, modifierName);
    }

    public async Task UnpublishEntityById(long entityId, string? modifierName)
    {
        var entity = await GetEntityById(entityId);
        if (entity != null)
        {
            UnpublishEntity(entity, modifierName);
        }
    }

    public void DeleteEntity(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteEntities(List<TEntity> entityIds)
    {
        _dbSet.RemoveRange(entityIds);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    #region Dispose

    public async ValueTask DisposeAsync()
    {
        // TODO release managed resources here
    }

    #endregion
}