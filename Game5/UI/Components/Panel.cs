using Game5.Data.Attributes.UI;
using Game5.Graphics;
using Game5.Input;
using Game5.Resource;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.UI.Components
{
    [UIDocElement("panel")]
    public class Panel : UserControl
    {
        private Point offset;
        private bool overflowX, overflowY, dragging;

        [UIDocField("scrollbarTexture")] public string ScrollBarTextureName { get; set; }

        [UIDocField("scrollbarGrabTexture")] public string ScrollBarGrabTextureName { get; set; }

        [UIDocField("grabcolor")] public Color GrabColor { get; set; }

        [UIDocField("scrollcolor")] public Color ScrollColor { get; set; }

        [UIDocField("scrollwidth")] public int ScrollWidth { get; set; }

        [UIDocField("scrollheight")] public int ScrollHeight { get; set; }

        protected Texture2D ScrollBarTexture => ServiceLocator.Get<IResourceService>().GetTexture(ScrollBarTextureName);

        protected Texture2D ScrollBarGrabTexture =>
            ServiceLocator.Get<IResourceService>().GetTexture(ScrollBarGrabTextureName);

        public override void Draw()
        {
            var matrix = Matrix.CreateTranslation(new Vector3(-offset.X, -offset.Y, 0));
            Children.ForEach(x => x.SetMatrix(matrix));
            BeginDraw();
            SpriteBatch.DrawRectangle(new Rectangle(GetRenderLocation(), DestRect.Size), BackColor, Texture);
            if (overflowY)
            {
                SpriteBatch.DrawRectangle(
                    new Rectangle(GetRenderLocation() + new Point(DestRect.Width - ScrollWidth, 0),
                        new Point(ScrollWidth, DestRect.Height)), ScrollColor, ScrollBarTexture);

                var factor = offset.Y / (GetMaxChildHeight() - (float) DestRect.Height);

                SpriteBatch.DrawRectangle(
                    new Rectangle(
                        GetRenderLocation() + new Point(DestRect.Width - ScrollWidth,
                            (int) ((DestRect.Height - ScrollHeight) * factor)), new Point(ScrollWidth, ScrollHeight)),
                    GrabColor, ScrollBarGrabTexture);
            }

            SpriteBatch.End();
            base.Draw();
        }

        public override void Update()
        {
            base.Update();

            if (GetMaxChildHeight() > DestRect.Height) overflowY = true;
            else overflowY = false;
            if (GetMaxChildWidth() > DestRect.Width) overflowX = true;
            else overflowX = false;

            if (overflowY && (Active || dragging))
            {
                var factor = offset.Y / (GetMaxChildHeight() - (float) DestRect.Height);
                var mousePos = GetTransformedMousePosition(Input.GetCurrentMouseState());
                var grabRect =
                    new Rectangle(
                        DestRect.Location + new Point(DestRect.Width - ScrollWidth,
                            (int) ((DestRect.Height - ScrollHeight) * factor)), new Point(ScrollWidth, ScrollHeight));
                var delta = Input.GetScrollDelta();

                if (IntersectsRect(mousePos, grabRect))
                    if (Input.IsClicked(MouseButton.LeftButton))
                        dragging = true;
                if (dragging && Input.IsDown(MouseButton.LeftButton))
                {
                    UserInterface.Active.ForEach(x => x.Active = false);
                    Active = true;
                    dragging = true;
                }

                if (Input.IsUp(MouseButton.LeftButton)) dragging = false;

                if (dragging)
                    offset.Y = (int) ((mousePos.Y - DestRect.Y) / (float) DestRect.Height *
                                      (GetMaxChildHeight() - DestRect.Height));

                offset.Y += (int) (delta / 10.0f);

                if (offset.Y > GetMaxChildHeight() - DestRect.Height) offset.Y = GetMaxChildHeight() - DestRect.Height;
                if (offset.Y < 0) offset.Y = 0;
            }
        }
    }
}