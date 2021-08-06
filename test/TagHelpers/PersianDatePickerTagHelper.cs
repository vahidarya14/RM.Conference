using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace test.TagHelpers
{
    [HtmlTargetElement("persianDatePicker", Attributes = DescriptionAttributeName, TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PersianDatePickerTagHelper : TagHelper
    {
        private const string DescriptionAttributeName = "asp-for";

        [HtmlAttributeName(DescriptionAttributeName)]
        public ModelExpression Model { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var dt = Model.Model != null ? (DateTime)Model.Model : DateTime.Now;
            dt = dt == DateTime.MinValue ? DateTime.Now : dt;
            var enabletimepicker = "true";
            output.Content.SetHtmlContent(
                 "<div class='input-group'" +
                 ">" +
                 "   <div class='input-group-addon'                           " +
                 "     data-mddatetimepicker='true'                           " +
                 "     data-targetselector='#" + Model.Name + "'              " +
                 "     data-trigger='click'                                   " +
                 "     data-enabletimepicker='"+enabletimepicker+"'           " +
                 "     data-isgregorian='false'                               " +
                 "     data-placement='bottom'                                " +
                 "     style='padding: 2px 12px;'>                            " +
                 "       <span class='la la-calendar la-2x'></span>           " +
                 "   </div>                                                   " +
                 "   <input type = 'text' class='form-control' id='prefix" + Model.Name + "' value='" + dt.ToPersian() + "'" +
                 "        data-mddatetimepicker='true'                        " +
                 "        data-targetselector='#" + Model.Name + "'           " +
                 "        data-trigger='click'                                " +
                 "        data-enabletimepicker='" + enabletimepicker + "'    " +
                 "        data-isgregorian='false'                            " +
                 "        data-placement='bottom'                             " +
                 "        data-englishnumber='false'                          " +
                 "                   />" +
                 "   <input type='hidden' id='" + Model.Name + "' name='" + Model.Name + "' value='" + dt + "' />" +
                 "   <script>" +
                 "        $(document).ready(function () {" +
                 "                $('#prefix" + Model.Name + "').change(function () {" +
                 "                    var selectedDate = $(this).val();" +
                 "                    $('#" + Model.Name + "').val(getGregorianDate(selectedDate));" +
                 "                });" +
                 "        });" +
                 "   </script>" +
                 "</div>"
                );



        }
    }

}
