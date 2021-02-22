using CasaDoCodigo.Core.Repository;
using CasaDoCodigo.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CasaDoCodigo.EF
{
    public class BookRepository : RepositoryBase<Book, int>, IBookRepository
    {
        public BookRepository(CasaDoCodigoDbContext dbContext)
            : base(dbContext, e => e.Id)
        {
        }

        public override IQueryable<Book> Query
            => Set()
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                .ThenInclude(a => a.Author);
    }
}
