using System;
using System.Collections.Generic;

namespace CasaDoCodigo.Model
{
    public class Book
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }
        public virtual string SubTitle { get; set; }
        public virtual string Summary { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string CoverUri { get; set; }
        public virtual decimal Price { get; set; }

        public virtual DateTime UpdateDate { get; set; }
        public virtual DateTime PublishDate { get; set; }

        public virtual ICollection<BookAuthorJoin> BookAuthors { get; set; }
        public virtual Category Category { get; set; }
    }
}
