using Microsoft.Xna.Framework;

namespace Game5.Data
{
    public class StyledString
    {
        public Color color;
        public string text;

        public StyledString(Color color, string text)
        {
            this.text = text;
            this.color = color;
        }
    }
}