using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.Graphics.Drawable
{
    public interface ISprite
    {
        void Draw(Vector2 position, float rotation, Color color, SpriteEffects effects, Vector2 origin, Vector2 scale);
    }
}