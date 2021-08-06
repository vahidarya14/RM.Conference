using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Laxor5.TagHelpers
{
    [HtmlTargetElement("EnumDropDown", Attributes = "asp-for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class EnumDropDownTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression Model { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var name = Model.Name;
            var value = (Enum)Model.Model;
            var values = Enum.GetValues(value.GetType());
            var enumType = value.GetType();
            var multi = context.AllAttributes.ContainsName("multiple");
            var onchange = context.AllAttributes["onchange"];

            var tag =
                "<select "+(multi ? " multiple ":"") +"class='form-control"+(!multi ? " bootstrap-select ":"")+"' id='" + name + "' name='" + name + "' " + (onchange!=null?"onchange ='"+onchange.Value+"'":"") + "  >";
            foreach (Enum item in values)
                tag += "  <option value ='" + item + "' " + (value.HasFlag((Enum)Enum.Parse(enumType, item.ToString())) ? "selected" : "") + " >" + item + " </option>";
            tag += "</select>" ;


            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(tag);



        }
    }
}
