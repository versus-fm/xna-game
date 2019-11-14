using System;

namespace Game5.Data.Attributes.Component
{
    public class AutoComponentAttribute : Attribute
    {
        public AutoComponentAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}