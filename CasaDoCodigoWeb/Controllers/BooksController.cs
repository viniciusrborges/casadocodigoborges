using AutoMapper;
using CasaDoCodigo.Core.Repository;
using CasaDoCodigo.Model;
using CasaDoCodigoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigoWeb.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Route("/books/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookRepository.GetAsync(id);
            if(book == null)
                return NotFound();

            var viewModel = _mapper.Map<BookViewModel>(book);
            return View(viewModel);
        }

        [Route("/category/{name}")]
        public IActionResult Category(string name)
        {
            var category = _categoryRepository.Query.FirstOrDefault(c => c.Name == name);
            if (category == null)
                return NotFound();

            return category.Parent == null
                ? MainCategory(category)
                : SubCategory(category);
        }

        private IActionResult MainCategory(Category category)
        {
            var _books = _bookRepository.Query;
            var categoryId = category.Id;
            var books = _books.Where(b => b.Category.Id == categoryId || b.Category.Parent.Id == categoryId);

            var bookViewModels = _mapper.Map<List<BookViewModel>>(books.ToList());
            var groups = bookViewModels.GroupBy(k => k.SubCategory ?? k.Category);
            var itemsByCategory = groups.ToDictionary(k => k.Key, k => k.Select(v => v));

            var vm = new BooksCategoryViewModel { CategoryName = category.Name, Items = itemsByCategory };

            return View(nameof(Category), vm);
            
        }

        private IActionResult SubCategory(Category category)
        {
            var _books = _bookRepository.Query;
            var categoryId = category.Id;
            var books = _books.Where(b => b.Category.Id == categoryId);
            var booksList = books.ToList();
            var bookViewModels = _mapper.Map<List<BookViewModel>>(books.ToList());

            var items = new Dictionary<string, IEnumerable<BookViewModel>> { { category.Name, bookViewModels } };
            var vm = new BooksCategoryViewModel { CategoryName = category.Parent.Name, Items = items };

            return View(nameof(Category), vm);
        }
    }
}
