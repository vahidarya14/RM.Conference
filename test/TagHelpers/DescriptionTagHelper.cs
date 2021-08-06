using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Laxor5.TagHelpers
{
    [HtmlTargetElement("description", Attributes = DescriptionAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DescriptionTagHelper : TagHelper
    {
        private const string DescriptionAttributeName = "asp-for";


        [HtmlAttributeName(DescriptionAttributeName)]
        public ModelExpression Model { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var description = GetDescription(Model.ModelExplorer);

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetContent(description);
        }

        private string GetDescription(ModelExplorer modelExplorer)
        {
            string description;
            description = modelExplorer.Metadata.Placeholder;

            if (String.IsNullOrWhiteSpace(description))
            {
                description = modelExplorer.Metadata.Description;
            }

            return description;
        }
    }
}
