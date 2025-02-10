using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task SaveAsync();
    }
}
