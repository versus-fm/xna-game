using Game5.Data.Attributes.Lua;
using Game5.UI.Components;
using MoonSharp.Interpreter;

namespace Game5.Data.LuaAPI.API.UserControls
{
    [LuaProxyClass(typeof(CheckButton))]
    public class LuaCheckButtonAPI : LuaUserControlAPI
    {
        public LuaCheckButtonAPI(object c) : base(c)
        {
        }

        private CheckButton check => (CheckButton) control;

        public Closure OnCheckChanged
        {
            get => check.OnCheckChanged;
            set => check.OnCheckChanged = value;
        }

        public string GetContent()
        {
            return check.Content;
        }

        public void SetContent(string content)
        {
            check.Content = content;
        }

        public bool IsChecked()
        {
            return check.Checked;
        }

        public void SetChecked(bool stat)
        {
            check.Checked = stat;
        }
    }
}