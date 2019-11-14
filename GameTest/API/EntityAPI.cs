using Game5.Data;
using Game5.Data.LuaAPI.API;
using MoonSharp.Interpreter;

namespace GameTest.API
{
    public class EntityAPI : LuaEntityAPI
    {
        [MoonSharpHidden]
        public EntityAPI(object e) : base(e)
        {
        }
    }
}