using CasaDoCodigo.Core.Repository;
using CasaDoCodigo.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasaDoCodigo.EF
{
    public class CategoryRepository : RepositoryBase<Category, int>, ICategoryRepository
    {
        public CategoryRepository(CasaDoCodigoDbContext dbContext)
            : base(dbContext, e => e.Id)
        {
        }

        public override IQueryable<Category> Query
            => Set()
                .Include(b => b.Parent);
    }
}
