using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace APIMusicPlayLists.Core.Interfaces.IRepositories
{
    public interface IRepositoryBase<T> 
    {
        Task<IEnumerable<T>> List();
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> QueryAbleList();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> Query();
        IQueryable<T> QuerySQL(string sql);

    }
}
