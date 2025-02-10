using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public ExpenseTrackerDbContext Context;
        public DbSet<TEntity> DbSet;

        public GenericRepository(ExpenseTrackerDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity); 
            await SaveAsync(); 
            return entity;
        }

        public async Task<TEntity> GetByIdAsync(int id) => await DbSet.FindAsync(id);

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync() => await Context.SaveChangesAsync();
    }
}
