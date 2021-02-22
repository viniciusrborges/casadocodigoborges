using System;
using System.Collections.Generic;
using System.Text;

namespace CasaDoCodigo.Model
{
    public class BookAuthorJoin
    {
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
