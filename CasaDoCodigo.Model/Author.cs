using System.Collections.Generic;

namespace CasaDoCodigo.Model
{
    public class Author
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<BookAuthorJoin> BookAuthors { get; set; }
    }
}