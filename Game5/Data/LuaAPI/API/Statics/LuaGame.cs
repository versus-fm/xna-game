using System.IO;
using Game5.Data.Attributes.Lua;
using Game5.Data.LuaAPI.AddonAPI;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Game5.UI;
using Game5.UI.Components;
using MoonSharp.Interpreter;

namespace Game5.Data.LuaAPI.API.Statics
{
    [LuaStaticClass("game")]
    public class LuaGame
    {
        public static void EnterState(string state, bool additive)
        {
            ServiceLocator.Get<IStateService>().PushState(state, additive);
        }

        public static void LeaveState()
        {
            ServiceLocator.Get<IStateService>().PopState();
        }

        public static UserControl GetControl(string s)
        {
            return UserInterface.Active.DeepFindControl(s);
        }

        public static UserControl CreateControl(string type, string name)
        {
            return UserInterface.CreateEmpty(type, name);
        }

        public static void AddControl(UserControl control)
        {
            UserInterface.Active.AddControl(control);
        }

        public static void LoadUserInterface(string file, string style = "")
        {
            var xml = File.ReadAllText(file + ".xml");
            UserInterface.FromDocument(UserInterface.Active, xml, style);
        }

        public static void SendMessage(int code)
        {
            ExtendedGame.SendMessage(code);
        }

        public static void Exit()
        {
            ExtendedGame.SendMessage(0);
        }

        public static void SubscribeEvent(string name, DynValue d)
        {
            LuaContext.Subscribe(name, d.Function);
        }

        public static void MakeCheckButtonGroup(params string[] names)
        {
            CheckButton.MakeGroup(names);
        }
    }
}