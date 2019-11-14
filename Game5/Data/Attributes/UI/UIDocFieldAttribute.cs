using System;

namespace Game5.Data.Attributes.UI
{
    public class UIDocFieldAttribute : Attribute
    {
        public UIDocFieldAttribute(string attrName)
        {
            AttributeName = attrName;
        }

        public string AttributeName { get; set; }
    }
}