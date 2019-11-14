using System;
using Game5.Data;
using Game5.Data.Attributes.UI;
using Game5.Data.Helper;
using Game5.Graphics;
using Game5.Input;
using Game5.Input.Interfaces;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game5.UI.Components
{
    [UIDocElement("textedit")]
    public class TextEdit : UserControl, ITextInput
    {
        private int index,
            start,
            timer;

        public TextEdit()
        {
            ServiceLocator.Get<ITextInputService>().HookTextInput(this);
        }

        [UIDocField("content")] public string Content { get; set; }

        [UIDocField("caretcolor")] public Color CaretColor { get; set; }

        public void OnTextInput(object sender, TextInputEventArgs args)
        {
            timer = 500;
            if (!Active) return;
            if (IsValid(args.Character))
            {
                Content = Content.Insert(index, args.Character.ToString());
                index++;
            }
            else
            {
                if (args.Character == 8 && Content.Length > 0 && index > 0)
                {
                    Content = Content.Remove(index - 1, 1);
                    index--;
                    if (index < start + 1)
                    {
                        start -= 5;
                        if (start < 0)
                            start = 0;
                    }
                }
            }
        }

        ~TextEdit()
        {
            ServiceLocator.Get<ITextInputService>().RemoveTextInput(this);
        }

        public override void Draw()
        {
            var gameTime = ServiceLocator.Get<IGameTimeService>().GetTime();
            var val = Math.Cos(gameTime.TotalGameTime.TotalSeconds * 5);
            var scale = FontHelper.GetFontScaling(Font, FontSize) * UserInterface.Active.Scale;
            var fontMeasure = Font.MeasureString("S") * scale;
            var textMeasure = Font.MeasureString(Content.Substring(start, index - start)) * scale;
            var renderLoc = GetRenderLocation();
            var vpos = new Vector2(renderLoc.X + FontSize / 2, renderLoc.Y + textMeasure.Y / 2);

            BeginDraw();
            SpriteBatch.DrawRectangle(new Rectangle(renderLoc, DestRect.Size), BackColor, Texture);
            SpriteBatch.DrawString(Font, Content.Substring(start, GetProperLength()), vpos, FontSize, ForeColor);
            if ((val > 0 || timer > 0) && Active)
                SpriteBatch.DrawRectangle(
                    new Rectangle((int) vpos.X + (int) textMeasure.X, (int) vpos.Y,
                        (int) (FontSize / 7 * UserInterface.Active.Scale), (int) fontMeasure.Y), CaretColor);
            SpriteBatch.End();
            base.Draw();
        }

        public override void Update()
        {
            var gameTime = ServiceLocator.Get<IGameTimeService>().GetTime();
            var scale = FontHelper.GetFontScaling(Font, FontSize) * UserInterface.Active.Scale;
            var fontMeasure = Font.MeasureString("S");
            var textMeasure = Font.MeasureString(Content.Substring(start, index - start)) * scale;
            var input = ServiceLocator.Get<IInput>();

            if (Active)
            {
                if (input.IsClicked(Keys.Left))
                {
                    timer = 500;
                    index--;
                    if (index < start + 1)
                    {
                        start -= 5;
                        if (start < 0)
                            start = 0;
                    }

                    if (index < 0)
                        index = 0;
                }

                if (input.IsClicked(Keys.Right))
                {
                    timer = 500;
                    index++;
                    if (index > Content.Length)
                        index = Content.Length;
                }

                if (input.IsClicked(Keys.Delete))
                {
                    timer = 500;
                    if (Content.Length > 0 && index < Content.Length) Content = Content.Remove(index, 1);
                }
            }

            var width = (int) textMeasure.X + FontSize / 2;

            while (width > DestRect.Width)
            {
                start += 2;
                width = (int) (Font.MeasureString(Content.Substring(start, index - start)) * scale).X + FontSize / 2;
            }

            timer -= (int) ServiceLocator.Get<IGameTimeService>().GetTime().ElapsedGameTime.TotalMilliseconds;
            base.Update();
        }

        private int GetProperLength()
        {
            var scale = FontHelper.GetFontScaling(Font, FontSize) * UserInterface.Active.Scale;
            var t = "";
            for (var i = start; i < Content.Length; i++)
            {
                t += Content[i];
                var measure = Font.MeasureString(t) * scale;
                if (measure.X + FontSize / 2 >= DestRect.Width) break;
            }

            return t.Length;
        }

        private bool IsValid(char c)
        {
            return char.IsLetterOrDigit(c) || char.IsSymbol(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c);
        }
    }
}