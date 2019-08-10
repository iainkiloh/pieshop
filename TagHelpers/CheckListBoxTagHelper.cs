using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pieshop.TagHelpers
{
    /// <summary>
    /// TagHelper For a Multi-Select Checkbox List
    /// The consuming view MUST use a @model Property which is of type List<SelectListItem>
    /// </summary>
    [HtmlTargetElement(Attributes = "asp-multichecklist-for, asp-items")]
    public class CheckListBoxTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-multichecklist-for")]
        public string ModelPropertyName { get; set; }

        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> AllItemOptions { get; set; }

        //used to access the consuming view context
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        //used to access existing bound property values found in the consuming views' model
        private IEnumerable<SelectListItem> _modelPropertyValues { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //attempt to get the existing PropertyValue (IEnumerable<SelectListItem>) from the consuming view (using the given ModelPropertyName)
            try
            {
                _modelPropertyValues = (IEnumerable<SelectListItem>)ViewContext.ViewData.ModelExplorer.GetExplorerForProperty(ModelPropertyName).Model;
            }
            catch(Exception e)
            {
                //not found initialized property error log error - not fatal - consuming view could be an 'Add New' @model type 
                Console.Error.WriteLine(e.Message);
                _modelPropertyValues = null;
            }

            //loop through the complete list of checkbox options
            var i = 0;
            foreach (var item in AllItemOptions)
            {
                var selected = item.Selected ? @"checked=""checked""" : "";
                var disabled = item.Disabled ? @"disabled=""disabled""" : "";

                //check for existing bound property in the Model, and set the selected and disabled properties if found
                if(_modelPropertyValues != null)
                {
                    //i choose to use both name and value propertires of the SelectedListItem, to allow for cases of Id and name being different (i.e. from a db lookup table for example)
                    var modelPropertyValueItem = _modelPropertyValues.Where(x => x.Text == item.Text && x.Value == item.Value).FirstOrDefault();
                    if(modelPropertyValueItem != null)
                    {
                        selected = modelPropertyValueItem.Selected ? @"checked=""checked""" : "";
                        disabled = modelPropertyValueItem.Disabled ? @"disabled=""disabled""" : "";
                    }
                }

                var html = $@"<label><input type=""checkbox"" {selected} {disabled} id=""{ModelPropertyName}_{i}__Selected"" name=""{ModelPropertyName}[{i}].Selected"" value=""true"" /> {item.Text}</label>";
                html += $@"<input type=""hidden"" id=""{ModelPropertyName}_{i}__Value"" name=""{ModelPropertyName}[{i}].Value"" value=""{item.Value}"">";
                html += $@"<input type=""hidden"" id=""{ModelPropertyName}_{i}__Text"" name=""{ModelPropertyName}[{i}].Text"" value=""{item.Text}"">";

                output.Content.AppendHtml(html);

                i++;
            }

            //output.Attributes.SetAttribute("class", "th-chklstbx");
        }
    }

}
