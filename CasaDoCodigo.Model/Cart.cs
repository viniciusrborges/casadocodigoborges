using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CasaDoCodigo.Model
{
    public class Cart
    {
        public decimal TotalPrice { get => Itens.Sum(b => b.Quantity * b.Book.Price); }
        public IEnumerable<CartItem> Itens { get; }

        public Cart(IEnumerable<CartItem> itens)
        {
            Itens = itens ?? throw new ArgumentNullException(nameof(itens));
        }
    }
}
