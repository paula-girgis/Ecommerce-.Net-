using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;

namespace WebApplication1.Core.Repositries
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T>? GetByIdAsync(Guid id);

        Task<int> AddAsync(T Entity);

        Task<int> UpdateAsync(T Entity);

        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteAsync(T Entity);
    }
}
