using System;

namespace Game5.Data.Attributes.Component
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComponentAttribute : Attribute
    {
        public ComponentAttribute(string name = "")
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}