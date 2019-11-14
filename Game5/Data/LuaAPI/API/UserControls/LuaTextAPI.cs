using Game5.Data.Attributes.Lua;
using Game5.UI.Components;

namespace Game5.Data.LuaAPI.API.UserControls
{
    [LuaProxyClass(typeof(Text))]
    public class LuaTextAPI : LuaUserControlAPI
    {
        public LuaTextAPI(object c) : base(c)
        {
        }

        private Text text => (Text) control;

        public string GetContent()
        {
            return text.Content;
        }

        public void SetContent(string content)
        {
            text.Content = content;
        }

        public void SetAutoSize(bool autosize)
        {
            text.AutoSize = autosize;
        }
    }
}