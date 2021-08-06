//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.AspNetCore.Razor.TagHelpers;
//using System.Collections.Generic;
//using A.FrameWork2.Models;

//namespace Laxor5.TagHelpers
//{
//    [HtmlTargetElement("Select2", Attributes = "asp-for,asp-items", TagStructure = TagStructure.NormalOrSelfClosing)]
//    public class Select2TagHelper : TagHelper
//    {
//        [HtmlAttributeName("asp-for")]
//        public ModelExpression Model { get; set; }

//        [HtmlAttributeName("asp-items")]
//        public List<TextIdModel> Items { get; set; }

//        public bool ForBus { get; set; }


//        public override void Process(TagHelperContext context, TagHelperOutput output)
//        {
//            var name = Model.Name;
//            var type = Model.Model?.GetType()?? typeof(long?);
//            var value = type == typeof(long) ? (long)Model.Model : type == typeof(long?) ? (long?)Model.Model : (int)Model.Model;
//            var onchange =  context.AllAttributes["onchange"];

//            var pre = "<select class='form-control bootstrap-select ' id='" + name + "' name='" + name + "' " + (onchange != null ? "onchange ='" + onchange.Value + "'" : "") + "  >";
//                //"   <option value ='' > -- انتخاب کنید -- </option>";
//            var post = "";
//            foreach (var item in Items)
//                post += "   <option value ='" + item.Id + "' " + (value== item.Id ? "selected" : "") + " >" + item .Text+ " </option>";
//            post += "</select>";

//            if(ForBus)
//                post += "<button type ='button' onclick=\"BusFilterModal('#" + name + "')\" class='btn btn-default' style='position: absolute;z-index: 1;margin-right: -40px;border-radius: 0;    border-radius: 4px 0px 0px 4px;' ><span class='la la-search'></span></button>";


//            output.TagName = null;
//            output.TagMode = TagMode.StartTagAndEndTag;
//            output.PreContent.SetHtmlContent(pre);
//            output.PostContent.SetHtmlContent(post);
//            //output.Content.SetHtmlContent(tag);
//        }

//    }

//}
