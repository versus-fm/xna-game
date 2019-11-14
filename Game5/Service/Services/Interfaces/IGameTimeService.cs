using Microsoft.Xna.Framework;

namespace Game5.Service.Services.Interfaces
{
    public interface IGameTimeService
    {
        float Delta();
        GameTime GetTime();
        void SupplyGameTime(GameTime gameTime);
    }
}