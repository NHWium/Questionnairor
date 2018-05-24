using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

//Modifier version of code found https://stackoverflow.com/questions/5955571/theres-no-html-button
namespace Questionnairor.Extensions
{
    public static class HtmlButtonExtensions
    {
        public static HtmlString Button(this HtmlHelper helper, string innerHtml, object htmlAttributes)
        {
            return Button(helper, innerHtml, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
    }
}