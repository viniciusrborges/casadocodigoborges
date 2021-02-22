using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CasaDoCodigoWeb.Models;
using CasaDoCodigo.Core.Repository;

namespace CasaDoCodigoWeb.Controllers
{
    // dotnet ef database update --project .\CasaDoCodigo.EF --startup-project .\CasaDoCodigoWeb
    public class HomeController : Controller
    {
        private IBookRepository _bookRepository;

        public HomeController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.FetchAllAsync();

            var updates = books.OrderByDescending(b => b.UpdateDate).Take(4).ToList();
            var publish = books.OrderByDescending(b => b.PublishDate).Take(4).ToList();

            var model = new HomeViewModel
            {
                FeaturedBig = publish[0],
                FeaturedMedium = publish[1],
                FeaturedSmall = publish[2],
                LastReleases = publish,
                LastUpdates = updates
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
