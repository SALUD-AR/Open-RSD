using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Msn.InteropDemo.Web.Helpers.Tags
{
    [HtmlTargetElement("titulo-pagina")]
    public class TituloPaginaTagHelper : TagHelper
    {
        private const string TituloTextAttributeName = "tituloText";
        private const string SubTituloTextAttributeName = "subTituloText";
        private const string TituloImageClassAttributeName = "imageClass";

        [HtmlAttributeName(TituloTextAttributeName)]
        public string TituloText { get; set; }

        [HtmlAttributeName(SubTituloTextAttributeName)]
        public string SubTituloText { get; set; }

        [HtmlAttributeName(TituloImageClassAttributeName)]
        public string TituloImageClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var st = "";
            if (!string.IsNullOrWhiteSpace(SubTituloText))
            {
                st = "&nbsp;/&nbsp;" + SubTituloText;
            }

            var strOut = $@"<h2><span class=""{TituloImageClass}"" style=""color:darkblue""></span> {TituloText}{st}</h2>";
            output.Content.AppendHtml(strOut);

            base.Process(context, output);
        }
    }
}
