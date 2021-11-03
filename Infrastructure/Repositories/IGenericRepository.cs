using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IGenericRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>();
        Task DeleteRowAsync<T>(T id);
        Task<T> GetAsync<T>(Guid id);
        Task<int> SaveRangeAsync<T>(IEnumerable<T> list);
        Task UpdateAsync<T>(T t);
        Task<int> InsertAsync<T>(T t);
        Task DeleteAsync();
    }
}
