using CasaDoCodigo.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasaDoCodigo.Core.Service
{
    public interface ICartService
    {
        void AddToCart(Book book);
        void AddToCart(Book book, int quantity);
        void RemoveFromCart(Book book);
        void DeleteFromCart(Book book);
        Cart GetCart();
    }
}
