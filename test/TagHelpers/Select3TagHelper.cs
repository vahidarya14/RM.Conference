using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace test.TagHelpers
{
    [HtmlTargetElement("Select3", Attributes = "asp-for,asp-items", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class Select3TagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression Model { get; set; }

        [HtmlAttributeName("asp-items")]
        public List<(string Id,string Text)> Items { get; set; }

        public bool HasModalBtn { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var name = Model.Name;
            var type = Model.Model?.GetType() ?? typeof(string);
            var value =Model.Model.ToString();
            var onchange = context.AllAttributes["onchange"];

            var pre = "<select class='form-control bootstrap-select ' id='" + name + "' name='" + name + "' " + (onchange != null ? "onchange ='" + onchange.Value + "'" : "") + "  >";
            //"   <option value ='' > -- انتخاب کنید -- </option>";
            var post = "";
            foreach (var item in Items)
                post += "   <option value ='" + item.Id + "' " + (value == item.Id ? "selected" : "") + " >" + item.Text + " </option>";
            post += "</select>";

            if (HasModalBtn)
                post += "<button type ='button' onclick=\"BusFilterModal('#" + name + "')\" class='btn btn-default' style='position: absolute;z-index: 1;margin-right: -40px;border-radius: 0;' ><span class='la la-search'></span></button>";


            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.PreContent.SetHtmlContent(pre);
            output.PostContent.SetHtmlContent(post);
            //output.Content.SetHtmlContent(tag);
        }

    }
}
