using System;

namespace Game5.Data.Attributes.Lua
{
    public class LuaStaticClassAttribute : Attribute
    {
        public LuaStaticClassAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     The name this class will occupy in the global table
        /// </summary>
        public string Name { get; set; }
    }
}