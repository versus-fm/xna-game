using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Game5.Data.Helper
{
    public static class ColorHelper
    {
        public static Color FromName(string name)
        {
            var col = ColorTranslator.FromHtml(name);
            return new Color(col.R, col.G, col.B, col.A);
        }

        public static string ToName(Color color)
        {
            var c = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            return ColorTranslator.ToHtml(c);
        }
    }
}