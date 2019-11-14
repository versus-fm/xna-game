using Game5.Data.Attributes.UI;
using Game5.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.UI.Components
{
    [UIDocElement("icon")]
    public class Icon : UserControl
    {
        public override void Draw()
        {
            BeginDraw(SamplerState.PointClamp);
            SpriteBatch.DrawRectangle(new Rectangle(GetRenderLocation(), DestRect.Size), BackColor, TextureBack);
            if (Texture != null)
                SpriteBatch.Draw(Texture, new Rectangle(GetRenderLocation(), DestRect.Size), ForeColor);
            SpriteBatch.End();
        }
    }
}