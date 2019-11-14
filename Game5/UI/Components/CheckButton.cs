using System.Collections.Generic;
using System.Linq;
using Game5.Data.Attributes.UI;
using Game5.Graphics;
using Game5.Resource;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;

namespace Game5.UI.Components
{
    [UIDocElement("checkButton")]
    public class CheckButton : Text
    {
        private bool _checked;
        private List<CheckButton> exclusiveGroup;

        public CheckButton()
        {
            overrideDefaultDraw = true;
            exclusiveGroup = new List<CheckButton>();
        }

        [UIDocField("checkWidth")]
        public int CheckTextureWidth
        {
            get => CheckedTextureSize.X;
            set => CheckedTextureSize = new Point(value, CheckedTextureSize.Y);
        }

        [UIDocField("checkHeight")]
        public int CheckTextureHeight
        {
            get => CheckedTextureSize.Y;
            set => CheckedTextureSize = new Point(CheckedTextureSize.X, value);
        }

        [UIDocField("insetWidth")]
        public int TextInsetWidth
        {
            get => TextInset.X;
            set => TextInset = new Point(value, TextInset.Y);
        }

        [UIDocField("insetHeight")]
        public int TextInsetHeight
        {
            get => TextInset.Y;
            set => TextInset = new Point(TextInset.Y, value);
        }

        public Point TextInset { get; set; }

        [UIDocField("checkedTexture")] public string CheckedTextureName { get; set; }

        [UIDocField("uncheckedTexture")] public string UnCheckedTextureName { get; set; }

        [UIDocField("checked")]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (value)
                    exclusiveGroup.ForEach(x =>
                    {
                        if (x != this && x.Checked) x.Checked = false;
                    });
                _checked = value;
                OnCheckChanged?.Call(this);
            }
        }

        public Point CheckedTextureSize { get; set; }
        public Closure OnCheckChanged { get; set; }

        public Texture2D CheckedTexture => ServiceLocator.Get<IResourceService>().GetTexture(CheckedTextureName);
        public Texture2D UnCheckedTexture => ServiceLocator.Get<IResourceService>().GetTexture(UnCheckedTextureName);

        public override void Draw()
        {
            var size = new Point((int) (CheckedTextureSize.X * UserInterface.Active.Scale),
                (int) (CheckedTextureSize.Y * UserInterface.Active.Scale));
            BeginDraw();
            if (Texture != null)
                SpriteBatch.DrawRectangle(new Rectangle(GetRenderLocation(), Size), BackColor, Texture);
            if (!Checked)
                SpriteBatch.DrawRectangle(new Rectangle(GetRenderLocation(), size), BackColor, UnCheckedTexture);
            else SpriteBatch.DrawRectangle(new Rectangle(GetRenderLocation(), size), BackColor, CheckedTexture);

            DrawString(new Vector2(TextInset.X, TextInset.Y) * UserInterface.Active.Scale);
            SpriteBatch.End();
            base.Draw();
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void MouseClick()
        {
            Checked = !Checked;
            base.MouseClick();
        }

        public void ClearGroup()
        {
            exclusiveGroup.Clear();
        }

        /// <summary>
        ///     Provides a method for turning a group of checkboxes into an exclusive group, emulating radiobutton behaviour
        /// </summary>
        /// <param name="radioButtons"></param>
        public static void MakeGroup(params CheckButton[] radioButtons)
        {
            var list = radioButtons.ToList();
            list.ForEach(x => x.exclusiveGroup = list);
        }

        /// <summary>
        ///     Provides a method for turning a group of checkboxes into an exclusive group, emulating radiobutton behaviour
        /// </summary>
        /// <param name="radioButtons"></param>
        public static void MakeGroup(params string[] radioButtons)
        {
            var list = radioButtons.ToList();
            var controls =
                new List<CheckButton>(list.Select(x => (CheckButton) UserInterface.Active.DeepFindControl(x)));
            controls.ForEach(x => x.exclusiveGroup = controls);
        }
    }
}