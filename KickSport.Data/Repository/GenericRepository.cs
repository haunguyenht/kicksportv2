using KickSport.Data.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KickSport.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable
        where TEntity : class
    {
        #region Properties
        public KickSportDbContext DbContext { get; }
        public IQueryable<TEntity> DbSet { get; }
        #endregion

        #region Constructor
        public GenericRepository(KickSportDbContext context)
        {
            DbContext = context;
            DbSet = context.Set<TEntity>();
        }
        #endregion

        #region Count
        public async Task<int> CountAsync()
            => await DbSet.CountAsync();

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria)
            => await DbSet.CountAsync(criteria);
        #endregion

        #region GetAll
        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await DbSet.ToListAsync();
        #endregion

        #region Find
        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> criteria)
            => await DbSet.FirstOrDefaultAsync(criteria);

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> criteria)
            => await DbSet.Where(criteria).ToListAsync();
        #endregion

        #region First and Last
        public async Task<TEntity> FirstAsync()
            => await DbSet.FirstAsync();
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> criteria)
            => await DbSet.FirstAsync(criteria);
        public async Task<TEntity> LastAsync()
            => await DbSet.LastAsync();
        public async Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> criteria)
            => await DbSet.LastAsync(criteria);
        #endregion

        #region Add
        public async Task AddAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbContext.Set<TEntity>().AddRangeAsync(entities);
        }
        #endregion

        #region Update
        public async Task Update(TEntity entity)
        {
            await Task.Run(() =>
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            });
        }

        public async Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> criteria)
        {
            var original = await FindOneAsync(criteria);
            await Task.Run(() =>
            {
                DbContext.Entry(original).CurrentValues.SetValues(entity);
            });
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() =>
            {
                DbContext.UpdateRange(entities);
            });
        }
        #endregion

        #region Delete
        public async Task Delete(TEntity entity)
        {
            await Task.Run(() =>
            {
                DbContext.Set<TEntity>().Remove(entity);
            });
        }

        public async Task Delete(Expression<Func<TEntity, bool>> criteria)
        {
            var entity = await DbSet.FirstAsync(criteria);
            await Task.Run(() =>
            {
                DbContext.Set<TEntity>().Remove(entity);
            });
        }

        public async Task DeleteRange(IEnumerable<TEntity> entities)
        {
            await Task.Run(() =>
            {
                DbContext.RemoveRange(entities);
            });
        }

        public async Task DeleteRangeAsync(Expression<Func<TEntity, bool>> criteria)
        {
            IEnumerable<TEntity> entities = DbSet.Where(criteria);
            foreach (TEntity entity in entities)
            {
                await Delete(entity);
            }
        }
        #endregion

        #region SaveChanges
        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            DbContext.Dispose();
        }
        #endregion
    }
}
