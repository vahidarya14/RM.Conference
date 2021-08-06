using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace test.TagHelpers
{
    [HtmlTargetElement("TinyMce", Attributes = "asp-for", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TinyMceTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression Model { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.SetHtmlContent(
            "<textarea  class='form-control' id='" + Model.Name + "' name='" + Model.Name + "' >" + Model.Model + "</textarea>" +
            "<script src='/lib/tinymce/tinymce.min.js'></script>" +
            "<script>" +
            "$(function () {" +
            "    tinyMCE.init({                                                                                                     " +
            "            selector: '#" + Model.Name + "',                                                                         " +
            //"            //mode: 'textareas'," +
            //"            //theme: 'modern'," +
            //"            //inline_styles: true," +
            "            menubar: false,                                                                                         " +
            "            fontsize_formats: '8pt 9pt 10pt 11pt 12pt 26pt 36pt',                                                   " +
            "            height: 400,                                                                                            " +
            "            width: '100%'," +
            "            language: 'fa_IR'," +
            "            directionality: 'rtl'," +
            "            autoresize_min_height: 200,                                                                             " +
            "            autoresize_max_height: 400,                                                                             " +
            "            plugins: [                                                                                              " +
            "                'advlist autolink autoresize directionality lists link image charmap print preview anchor',         " +
            "                'searchreplace visualblocks code fullscreen textcolor',                                             " +
            "                'insertdatetime media table contextmenu paste fullpage'                                             " +
            "            ],                                                                                                      " +
            "            directionality :'rtl',                                                                                  " +
            "            toolbar:                                                                                                " +
            "                'fullscreen | undo redo | forecolor backcolor |ltr rtl | styleselect | fontselect | fontsizeselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link'," +
            "        });" +
            "});" +
            "</script>");
        }
    }
}
