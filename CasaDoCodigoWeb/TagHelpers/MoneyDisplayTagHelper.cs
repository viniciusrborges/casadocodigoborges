using CasaDoCodigoWeb.Helpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigoWeb.TagHelpers
{
    // <moneyDisplay> </moneyDisplay>
    public class MoneyDisplayTagHelper : TagHelper
    {
        public decimal Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = StringHelper.ToMoneyDisplay(Value);

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetContent(content);

            // base.Process(context, output);
        }
    }
}
