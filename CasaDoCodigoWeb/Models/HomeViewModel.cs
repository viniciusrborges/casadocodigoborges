using CasaDoCodigo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigoWeb.Models
{
    public class HomeViewModel
    {
        public Book FeaturedBig { get; set; }
        public Book FeaturedMedium { get; set; }
        public Book FeaturedSmall { get; set; }

        public IEnumerable<Book> LastReleases { get; set; }
        public IEnumerable<Book> LastUpdates { get; set; }
    }
}
