using APIMusicPlayLists.Core.Base;
using APIMusicPlayLists.Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIMusicPlayList.Infra.Data.EF.Data
{
    public class EFRepository<T> : IRepositoryBase<T> where T : BaseEntity
    {
        public readonly MusicPlayListDBContext _dbContext;
        public EFRepository(MusicPlayListDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext
                .Set<T>()
                //.AsNoTracking()
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ListAsync(int index, int pagesize)
        {
            return await _dbContext.Set<T>()
                .Skip(index)
                .Take(pagesize)
                .ToListAsync();
        }        
        
        public async Task<IEnumerable<T>> List()
        {
            return await _dbContext.Set<T>()
                .ToListAsync();
        }
                
        public IEnumerable<T> QueryAbleList()
        {
            return _dbContext.Set<T>()
                .ToList();
        }

        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>();
        }

        public IQueryable<T> QuerySQL(string sql)
        {
            var result = _dbContext.Set<T>().FromSqlRaw(sql);
            return result;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

        }        
 

    }
}
