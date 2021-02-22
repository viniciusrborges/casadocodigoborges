using System;
using System.Collections.Generic;
using System.Text;

namespace CasaDoCodigo.Model
{
    public class CartItem
    {
        public Book Book { get; }
        public int Quantity { get; }
        public decimal Price => Book.Price;
        public decimal TotalPrice => Book.Price * Quantity;

        public CartItem(Book book, int quantity)
        {
            Book = book ?? throw new ArgumentNullException(nameof(book));
            Quantity = quantity >= 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity));
        }
    }
}
