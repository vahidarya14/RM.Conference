using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Laxor5.TagHelpers
{
    [HtmlTargetElement("form-group", Attributes = "asp-for,asp-label", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class FormGroupTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression Model { get; set; }

        public string AspLabel { get; set; }




        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var name = Model.Name;
            var value = Model.Model;
            var type = Model.Model?.GetType() ?? typeof(string);
          

            if (type == typeof(bool))
            {
                var boolVal = bool.Parse(value.ToString());
                output.PreContent.SetHtmlContent(
                        " <div class='form-group checkbox-form-group'>" +
                        "    <label>" + AspLabel + "</label>" +
                        "    <div class='TriSea-technologies-Switch pull-right'>" +
                        "        <input " + (boolVal ? "checked" : "") + " id='" + name + "' name='" + name + "' class='form-control' type='checkbox' value='true' />" +
                        "        <label for='" + name + "' class='label-primary'></label>" +
                        "    </div>"
                   ) ;
                output.PostContent.SetHtmlContent("</div>");
            }
            else
            {
                var textarea = context.AllAttributes.ContainsName("textarea");
                var rows = textarea ? context.AllAttributes["textarea"].Value?.ToString()??"2" : "";
                output.Content.SetHtmlContent(
                    "<div class='form-group'>" +
                    "   <label asp-for='" + name + "'>" + AspLabel + "</label>" +
                    (
                    textarea?
                    "   <textarea id='" + name + "' name='" + name + "' class='form-control' rows="+ rows + " >" + value + "</textarea>" :
                    "   <input id='" + name + "' name='" + name + "' value='" + value + "' class='form-control' />"
                    ) +
                    "   <span asp-validation-for='" + name + "' class='text-danger'></span>" +
                    "</div>"
                   );
            }
               

        }
    }
}
