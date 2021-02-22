using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigoWeb.Helpers
{
    public static class StringHelper
    {
        private static CultureInfo _culture = CultureInfo.GetCultureInfo("pt-BR");

        public static string ToMoneyDisplay(decimal value) =>
            value.ToString("C", _culture);
    }
}
