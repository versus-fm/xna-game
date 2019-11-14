using Game5.Data.Attributes.Service;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IGameTimeService))]
    public class GameTimeService : IGameTimeService
    {
        private GameTime gameTime;

        public void SupplyGameTime(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

        public GameTime GetTime()
        {
            return gameTime;
        }

        public float Delta()
        {
            return 16.0f / gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}