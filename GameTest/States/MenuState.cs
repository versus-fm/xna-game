using System;
using Game5.Data.Attributes;
using Game5.Data.Attributes.State;
using Game5.StateBased;

namespace GameTest.States
{
    [State("menu")]
    public class MenuState : GameState
    {
        public override void Cleanup()
        {
        }

        public override void DrawWorld()
        {
        }

        public override string GetName()
        {
            return "menu";
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