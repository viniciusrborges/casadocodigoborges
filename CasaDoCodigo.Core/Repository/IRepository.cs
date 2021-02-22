using CasaDoCodigo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaDoCodigo.Core.Repository
{
    public interface IBookRepository : IRepository<Book, int> { }
    public interface ICategoryRepository : IRepository<Category, int> { }
    public interface IAuthorRepository : IRepository<Author, int> { }

    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        Task<List<TEntity>> FetchAllAsync();

        IQueryable<TEntity> Query { get; }

        Task<TEntity> GetAsync(TKey key);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);

        Task<int> SaveAsync();
    }
}
