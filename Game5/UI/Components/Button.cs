using Game5.Data.Attributes.UI;
using Game5.Graphics;
using Game5.Resource;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.UI.Components
{
    /// <summary>
    ///     A different element name to represent buttons rather than text, functionally identical. Also used to distinguish
    ///     for styling purposes
    /// </summary>
    [UIDocElement("button")]
    public class Button : Text
    {
        private string activeTexture;

        public Button()
        {
            overrideDefaultDraw = true;
        }

        [UIDocField("pressedTexture")] public string ButtonPressTextureName { get; set; }

        private Texture2D active => ServiceLocator.Get<IResourceService>().GetTexture(activeTexture);

        public override void Update()
        {
            activeTexture = TextureName;
            base.Update();
        }

        protected override void MouseDown()
        {
            if (!string.IsNullOrEmpty(ButtonPressTextureName)) activeTexture = ButtonPressTextureName;
            base.MouseDown();
        }

        public override void Draw()
        {
            BeginDraw();
            SpriteBatch.DrawRectangle(new Rectangle(GetRenderLocation(), DestRect.Size), BackColor, active);
            DrawString();
            SpriteBatch.End();
            base.Draw();
        }
    }
}