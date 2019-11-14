using System.Collections.Generic;
using Game5.Data.Attributes.Service;
using Game5.Data.LuaAPI;
using Game5.Data.LuaAPI.AddonAPI;
using Game5.Service.Services.Interfaces;
using Game5.StateBased;
using Game5.UI;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IStateService))]
    public class StateService : IStateService
    {
        private readonly StateStack stateStack;

        public StateService()
        {
            stateStack = new StateStack();
        }

        public void DrawUI()
        {
            var states = GetStates();
            for (var i = states.Count - 1; i >= 0; i--) states[i]?.DrawUI();
        }

        public void FinalizeDrawUI()
        {
            var states = GetStates();
            for (var i = states.Count - 1; i >= 0; i--) states[i]?.FinalizeUI();
        }

        public void DrawWorld()
        {
            var states = GetStates();
            for (var i = states.Count - 1; i >= 0; i--) states[i]?.DrawWorld();
        }

        public void Update()
        {
            var states = GetStates();
            for (var i = states.Count - 1; i >= 0; i--) states[i]?.Update();
        }

        public void UpdateUI()
        {
            var states = GetStates();
            for (var i = states.Count - 1; i >= 0; i--) states[i]?.UpdateUI();
        }

        public void PushState(GameState state, bool additive)
        {
            UserInterface.ChangeActive(state.UI);
            state.Init();
            LuaContext.FireEvent(LuaConstants.STATE_ENTER, state.GetName());
            state.PostAddonInit();
            stateStack.Push(state, additive);
        }

        /// <summary>
        ///     Slightly worse performance than using a type directly, the state also needs an empty constructor
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="additive"></param>
        public void PushState(string stateName, bool additive)
        {
            var state = GameState.GetState(stateName);
            UserInterface.ChangeActive(state.UI);
            state.Init();
            LuaContext.FireEvent(LuaConstants.STATE_ENTER, state.GetName());
            state.PostAddonInit();
            stateStack.Push(state, additive);
        }

        public GameState PopState()
        {
            if (stateStack.Count == 0) return null;
            var state = stateStack.Pop();
            state.Cleanup();
            return state;
        }

        public GameState GetState()
        {
            if (stateStack.Count == 0) return null;
            return stateStack.Peek();
        }

        public List<GameState> GetStates()
        {
            if (stateStack.Count == 0) return null;
            return stateStack.GetAdditiveStates();
        }
    }
}