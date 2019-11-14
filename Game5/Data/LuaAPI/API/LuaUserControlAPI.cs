using System;
using Game5.Data.Attributes.Lua;
using Game5.Data.Helper;
using Game5.UI;
using MoonSharp.Interpreter;

namespace Game5.Data.LuaAPI.API
{
    [LuaProxyClass(typeof(UserControl))]
    public class LuaUserControlAPI
    {
        protected UserControl control;

        [MoonSharpHidden]
        public LuaUserControlAPI(object c)
        {
            control = (UserControl) c;
        }

        public Closure OnClick
        {
            get => control.OnClick;
            set => control.OnClick = value;
        }

        public Closure OnHover
        {
            get => control.OnHover;
            set => control.OnHover = value;
        }

        public Closure OnDown
        {
            get => control.OnDown;
            set => control.OnDown = value;
        }

        public Closure OnLeave
        {
            get => control.OnLeave;
            set => control.OnLeave = value;
        }

        public Closure OnEnter
        {
            get => control.OnEnter;
            set => control.OnEnter = value;
        }

        public string GetBackColor()
        {
            return ColorHelper.ToName(control.BackColor);
        }

        public string GetForeColor()
        {
            return ColorHelper.ToName(control.ForeColor);
        }

        public void SetBackColor(string color)
        {
            control.BackColor = ColorHelper.FromName(color);
        }

        public void SetForeColor(string color)
        {
            control.ForeColor = ColorHelper.FromName(color);
        }

        public void AddChild(UserControl control)
        {
            control.AddChild(control);
        }

        public void SetAnchor(string anchor)
        {
            control.Anchor = (AnchorPoint) Enum.Parse(typeof(AnchorPoint), anchor);
        }

        public void SetSize(string width, string height)
        {
            control.DynamicWidth = width;
            control.DynamicHeight = height;
            control.MarkDirty();
        }

        public int GetWidth()
        {
            return control.Width;
        }

        public int GetHeight()
        {
            return control.Height;
        }

        public void SetTexture(string name)
        {
            control.TextureName = name;
        }

        public void SetFont(string font)
        {
            control.FontName = font;
        }

        public void SetVisible(bool visible)
        {
            control.Visible = visible;
        }

        public bool IsVisible()
        {
            return control.Visible;
        }

        public string GetName()
        {
            return control.Name;
        }

        public void Dirty()
        {
            control.MarkDirty();
        }
    }
}