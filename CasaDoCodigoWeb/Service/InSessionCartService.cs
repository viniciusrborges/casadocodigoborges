using CasaDoCodigo.Core.Repository;
using CasaDoCodigo.Core.Service;
using CasaDoCodigo.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigoWeb.Service
{
    public class InSessionCartService : ICartService
    {
        private const string CART_SESSION_PREFIX = "SessionCartService_";

        private readonly IBookRepository _bookRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public InSessionCartService(IHttpContextAccessor httpContextAccessor, IBookRepository bookRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public void AddToCart(Book book, int quantity)
        {
            ThrowIfNull(book, nameof(book));

            var bookKey = GetBookSessionKey(book);
            _httpContextAccessor.HttpContext.Session.SetInt32(bookKey, quantity);
        }

        public void AddToCart(Book book)
        {
            ThrowIfNull(book, nameof(book));

            var bookKey = GetBookSessionKey(book);
            var bookInCart = _httpContextAccessor.HttpContext.Session.GetInt32(bookKey);

            var bookKeyValue = bookInCart.HasValue
                ? bookInCart.Value + 1
                : 1;

            _httpContextAccessor.HttpContext.Session.SetInt32(bookKey, bookKeyValue);
        }

        public void RemoveFromCart(Book book)
        {
            ThrowIfNull(book, nameof(book));

            var bookKey = GetBookSessionKey(book);
            var bookInCart = _httpContextAccessor.HttpContext.Session.GetInt32(bookKey);

            if (!bookInCart.HasValue)
                return;

            var newValue = bookInCart.Value - 1;
            if (newValue <= 0)
                _httpContextAccessor.HttpContext.Session.Remove(bookKey);
            else
                _httpContextAccessor.HttpContext.Session.SetInt32(bookKey, newValue);
        }

        public void DeleteFromCart(Book book)
        {
            ThrowIfNull(book, nameof(book));

            var bookKey = GetBookSessionKey(book);
            _httpContextAccessor.HttpContext.Session.Remove(bookKey);
        }
        
        public Cart GetCart()
        {
            var books = (from key in _httpContextAccessor.HttpContext.Session.Keys
                         where key.StartsWith(CART_SESSION_PREFIX)
                         let bookIdPortion = key.Replace(CART_SESSION_PREFIX, String.Empty)
                         let bookId = Int32.Parse(bookIdPortion)
                         let quantity = _httpContextAccessor.HttpContext.Session.GetInt32(key).Value
                         select (bookId, quantity)).ToDictionary(b => b.bookId, b => b.quantity);

            var onlyKeys = books.Select(b => b.Key).ToArray();
            var result = _bookRepository.Query.Where(b => onlyKeys.Contains(b.Id));
            
            var itens =  from item in result
                         select new CartItem(item, books[item.Id]);

            return new Cart(itens);
        }

        private static void ThrowIfNull(object reference, string argumentName)
        {
            if (reference == null)
                throw new ArgumentNullException(argumentName);
        }

        private string GetBookSessionKey(Book book) => $"{CART_SESSION_PREFIX}{book.Id}";
    }
}
