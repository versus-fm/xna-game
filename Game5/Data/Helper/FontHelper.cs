using Microsoft.Xna.Framework.Graphics;

namespace Game5.Data.Helper
{
    public static class FontHelper
    {
        public static float GetFontScaling(SpriteFont font, int fontSize)
        {
            return fontSize / (float) font.GetGlyphs()['S'].BoundsInTexture.Height;
        }
    }
}