using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderService.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<T> GetByIdAsync(string id);
        Task<List<T>> ListAsync();
        Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(string id);
    }
}
