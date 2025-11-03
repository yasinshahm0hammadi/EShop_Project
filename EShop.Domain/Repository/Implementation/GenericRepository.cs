using EShop.Domain.Context;
using EShop.Domain.Entities.Common;
using EShop.Domain.Repository.Interface;
using Microsoft.EntityFrameworkCore;

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

    public async Task AddEntity(TEntity entity)
    {
        entity.CreateDate = DateTime.Now;
        entity.LastUpdateDate = DateTime.Now;
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeEntity(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            await AddEntity(entity);
        }
    }

    public async Task<TEntity> GetEntityById(long entityId)
    {
        return await _dbSet.SingleOrDefaultAsync(x => x.Id == entityId);
    }

    public void EditEntity(TEntity entity)
    {
        entity.LastUpdateDate = DateTime.Now;
        _dbSet.Update(entity);
    }

    public void EditEntityByEditor(TEntity entity, string editorName)
    {
        entity.LastUpdateDate = DateTime.Now;
        entity.editorName = editorName;
        _dbSet.Update(entity);
    }

    public void DeleteEntity(TEntity entity)
    {
        entity.IsDelete = true;
        EditEntity(entity);
    }

    public async Task DeleteEntityById(long entityId)
    {
        var entity = await GetEntityById(entityId);
        if (entity != null)
        {
            DeleteEntity(entity);
        }
    }

    public void DeletePermanentEntity(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeletePermanentEntities(List<TEntity> entityIds)
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