using System;
using Game5.Data.Attributes;
using Game5.Data.Attributes.State;
using Game5.StateBased;

namespace GameTest.States
{
    [State("lobby")]
    public class LobbyState : GameState
    {
        public override void Cleanup()
        {
        }

        public override void DrawWorld()
        {
        }

        public override string GetName()
        {
            return "lobby";
        }

        public override void Init()
        {
        }

        public override void PostAddonInit()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
        }
    }
}