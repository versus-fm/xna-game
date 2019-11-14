using System.Collections.Generic;
using Game5.StateBased;

namespace Game5.Service.Services.Implementations
{
    /// <summary>
    ///     Special "stack" for states that supports getting multiple states
    /// </summary>
    public class StateStack
    {
        private readonly List<StateInfo> states;

        public StateStack()
        {
            states = new List<StateInfo>();
        }

        public int Count => states.Count;

        public List<GameState> GetAdditiveStates()
        {
            var list = new List<GameState>();
            if (states.Count == 0) return list;
            for (var i = states.Count - 1; i >= 0; i--)
            {
                list.Add(states[i].state);
                if (!states[i].additive) break;
            }

            return list;
        }

        public void Push(GameState state, bool additive)
        {
            states.Add(new StateInfo(state, additive));
        }

        public GameState Pop()
        {
            if (Count == 0) return null;
            var state = states[Count - 1];
            states.RemoveAt(Count - 1);
            return state.state;
        }

        public GameState Peek()
        {
            if (Count == 0) return null;
            var state = states[Count - 1];
            return state.state;
        }
    }
}