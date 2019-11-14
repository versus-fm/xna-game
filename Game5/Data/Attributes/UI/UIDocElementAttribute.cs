using System;

namespace Game5.Data.Attributes.UI
{
    public class UIDocElementAttribute : Attribute
    {
        public UIDocElementAttribute(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; set; }
    }
}