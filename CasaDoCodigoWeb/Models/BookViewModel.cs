using AutoMapper;
using CasaDoCodigo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigoWeb.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }

        public string Authors { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Summary { get; set; }
        public string DisplayName { get; set; }
        public string CoverUri { get; set; }
        public decimal Price { get; set; }

        public DateTime UpdateDate { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
