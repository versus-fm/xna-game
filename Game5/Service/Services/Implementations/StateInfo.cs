using Game5.StateBased;

namespace Game5.Service.Services.Implementations
{
    public struct StateInfo
    {
        public GameState state;
        public bool additive;

        public StateInfo(GameState state, bool additive)
        {
            this.state = state;
            this.additive = additive;
        }
    }
}