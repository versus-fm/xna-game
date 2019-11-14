using System;

namespace Game5.Data.Attributes.State
{
    public class StateAttribute : Attribute
    {
        public StateAttribute(string name)
        {
            StateName = name;
        }

        public string StateName { get; set; }
    }
}