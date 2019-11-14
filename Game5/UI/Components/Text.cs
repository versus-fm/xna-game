using System;
using Game5.Data;
using Game5.Data.Attributes.UI;
using Game5.Data.Helper;
using Game5.Graphics;
using Game5.UI.UIEventArgs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game5.UI.Components
{
    [UIDocElement("text")]
    public class Text : UserControl
    {
        private FormattedString formattedString;

        private Color lastColor;

        protected bool overrideDefaultDraw = false;
        private string text;

        [UIDocField("content")]
        public string Content
        {
            get => text;
            set
            {
                var lastText = text;
                var lastFormat = formattedString;

                text = value;
                formattedString = FormattedString.BuildString(text, ForeColor);
                VisibleContent = formattedString.GetVisibleString();

                OnContentChanged?.Invoke(this, new TextChangedArgs(lastText, text, lastFormat, formattedString));

                if (AutoSize)
                {
                    var scaling = FontHelper.GetFontScaling(Font, FontSize) * UserInterface.Active.Scale;
                    var wholeMeasure = Font.MeasureString(VisibleContent) * scaling;
                    width = (int) wholeMeasure.X;
                    height = (int) wholeMeasure.Y;
                }

                MarkDirty();
            }
        }

        [UIDocField("styled")] public bool Styled { get; set; }

        [UIDocField("autosize")] public bool AutoSize { get; set; }

        public string VisibleContent { get; private set; }

        public event EventHandler OnContentChanged;

        public override void Update()
        {
            var scaling = FontHelper.GetFontScaling(Font, FontSize) * UserInterface.Active.Scale;
            var wholeMeasure = Font.MeasureString(VisibleContent) * scaling;
            if (AutoSize)
            {
                width = (int) wholeMeasure.X;
                height = (int) wholeMeasure.Y;
            }

            if (lastColor != ForeColor) formattedString = FormattedString.BuildString(text, ForeColor);
            lastColor = ForeColor;
            base.Update();
        }

        public override void Draw()
        {
            if (!overrideDefaultDraw)
            {
                BeginDraw();
                SpriteBatch.DrawRectangle(new Rectangle(GetRenderLocation(), DestRect.Size), BackColor, Texture);
                DrawString();
                SpriteBatch.End();
            }

            base.Draw();
        }

        protected void DrawString(Vector2 offset = default(Vector2))
        {
            var scaling = FontHelper.GetFontScaling(Font, FontSize) * UserInterface.Active.Scale;
            var stringMeasure = Font.MeasureString(VisibleContent) * scaling;
            var pos = GetRenderLocation();
            var vpos = new Vector2(pos.X + (DestRect.Width - stringMeasure.X) / 2,
                           pos.Y + DestRect.Height / 2 - stringMeasure.Y / 2) + offset;
            if (Styled)
                SpriteBatch.DrawFormattedString(Font, formattedString, vpos, scaling);
            else
                SpriteBatch.DrawString(Font, VisibleContent, vpos, ForeColor, 0f, Vector2.Zero, scaling,
                    SpriteEffects.None, 0);
        }
    }
}