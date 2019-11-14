using System;
using Game5;
using Game5.Data.Attributes;
using Game5.Data.Attributes.State;
using Game5.Env;
using Game5.Graphics;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Game5.StateBased;
using Microsoft.Xna.Framework.Graphics;

namespace GameTest.States
{
    [State("game")]
    public class GameplayState : GameState
    {
        private Entity player;

        public override void Cleanup()
        {
        }

        public override void DrawWorld()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                ServiceLocator.Get<ICamera>().GetMatrix());
            //world.Draw();
            SpriteBatch.End();
        }

        public override string GetName()
        {
            return "game";
        }

        public override void Init()
        {
        }

        public override void PostAddonInit()
        {
        }

        public override void Update()
        {
            //Console.WriteLine(player.Position);
            //world.Update();
        }
    }
}