using System;

namespace Game5.Data.Attributes.Lua
{
    public class LuaProxyClassAttribute : Attribute
    {
        public LuaProxyClassAttribute(Type type)
        {
            Target = type;
        }

        public Type Target { get; set; }
    }
}