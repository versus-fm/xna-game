using System;
using System.Collections.Generic;
using Game5.Data;
using Game5.Data.Attributes;
using Game5.Data.Attributes.State;
using Game5.Data.Helper;
using Game5.Input;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Game5.UI;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.StateBased
{
    public abstract class GameState
    {
        private static Dictionary<string, Type> states;
        private UserInterface _active;

        protected IStateService StateService => ServiceLocator.Get<IStateService>();

        protected IInput Input => ServiceLocator.Get<IInput>();

        protected IGameTimeService GameTime => ServiceLocator.Get<IGameTimeService>();

        protected SpriteBatch SpriteBatch => ServiceLocator.Get<SpriteBatch>();

        public bool TopMost => StateService.GetState() == this;

        public UserInterface UI
        {
            get
            {
                if (_active == null) _active = new UserInterface();
                return _active;
            }
            set => _active = value;
        }

        public abstract void Init();
        public abstract void PostAddonInit();
        public abstract void Cleanup();
        public abstract void DrawWorld();

        public void DrawUI()
        {
            UI.PreDraw();
        }

        public void FinalizeUI()
        {
            UI.PostDraw();
        }

        public void UpdateUI()
        {
            UI.Update();
        }

        public abstract void Update();
        public abstract string GetName();

        public static void RegisterStates()
        {
            states = new Dictionary<string, Type>();
            var types = TypeHelper.FindAllTypesWithAttribute<StateAttribute>();
            foreach (var type in types) states.Add(type.attribute.StateName, type.type);
        }

        public static GameState GetState(string name)
        {
            return (GameState) Activator.CreateInstance(states[name]);
        }
    }
}