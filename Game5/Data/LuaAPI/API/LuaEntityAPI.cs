using System.Collections.Generic;
using Game5.Data.Attributes.Lua;
using Game5.Env;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace Game5.Data.LuaAPI.API
{
    [LuaProxyClass(typeof(Entity))]
    public class LuaEntityAPI
    {
        private readonly Entity entity;

        [MoonSharpHidden]
        public LuaEntityAPI(object e)
        {
            entity = (Entity) e;
        }

        public List<Entity> GetEntities(int x, int y, int w, int h)
        {
            return null; // entity.world.QueryEntities(new Rectangle(x, y, w, h));
        }

        //public float GetX()
        //{
        //    return entity.Position.X;
        //}

        //public float GetY()
        //{
        //    return entity.Position.Y;
        //}

        //public float GetWidth()
        //{
        //    return entity.Size.X;
        //}

        //public float GetHeight()
        //{
        //    return entity.Size.Y;
        //}

        //public void Move(int x, int y)
        //{
        //    entity.Position += new Vector2(x, y);
        //}

        //public void SetPosition(int x, int y)
        //{
        //    entity.Position = new Vector2(x, y);
        //}
    }
}