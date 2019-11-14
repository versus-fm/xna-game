using System;
using Game5;
using Game5.Data.Helper;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Game5.StateBased;
using GameTest.States;

namespace GameTest
{
    public class MainGame : ExtendedGame
    {
        protected override void LoadGameContent()
        {
            Console.WriteLine("HelloWorldTest".ToSnakeCase());
            IsMouseVisible = true;
            var states = ServiceLocator.Get<IStateService>();
            states.PushState(new InitializationState(), false);
            base.LoadGameContent();
        }
    }
}