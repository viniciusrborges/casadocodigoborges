using CasaDoCodigo.Core.Repository;
using CasaDoCodigo.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasaDoCodigo.EF
{
    public class AuthorRepository: RepositoryBase<Author, int>, IAuthorRepository
    {
        public AuthorRepository(CasaDoCodigoDbContext dbContext)
            : base(dbContext, e => e.Id)
        {
        }
    }
}
