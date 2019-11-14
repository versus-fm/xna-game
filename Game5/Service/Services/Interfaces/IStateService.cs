using System.Collections.Generic;
using Game5.StateBased;

namespace Game5.Service.Services.Interfaces
{
    public interface IStateService
    {
        void DrawUI();
        void DrawWorld();
        void FinalizeDrawUI();
        GameState GetState();
        List<GameState> GetStates();
        GameState PopState();
        void PushState(GameState state, bool additive);
        void PushState(string stateName, bool additive);
        void Update();
        void UpdateUI();
    }
}