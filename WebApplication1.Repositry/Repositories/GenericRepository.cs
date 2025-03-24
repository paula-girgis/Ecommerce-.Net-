using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;
using WebApplication1.Repositry.Data;

namespace WebApplication1.Repositry.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly EcommerceContext context;

        public GenericRepository(EcommerceContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(T Entity)
        {
            await context.AddAsync(Entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return 0; // Or throw an exception if needed
            }
            context.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T Entity)
        {
            context.Remove(Entity);
            return await context.SaveChangesAsync();
        }




        public async Task<T>? GetByIdAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id);

        }

        public async Task<int> UpdateAsync(T Entity)
        {
            context.Update(Entity);
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().AsNoTracking().ToListAsync();
        }


    }
}
