using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LanchoMac.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public string endereco { get; set; }
        public string conteudo { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", "mailto:" + endereco);
            output.Content.SetContent(conteudo);
        }
    }
}
