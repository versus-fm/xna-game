using MoonSharp.Interpreter;

namespace Game5.Data.LuaAPI.AddonAPI
{
    public class Addon
    {
        private readonly AddonConfiguration config;
        private readonly DynValue mainFunction;
        private readonly Script script;

        public Addon(string name, Script script, AddonConfiguration config)
        {
            Name = name;
            this.script = script;
            this.config = config;

            mainFunction = script.Globals.Get(string.IsNullOrWhiteSpace(this.config.MainFunction)
                ? this.config.MainFunction
                : "main");
            if (mainFunction == null) mainFunction = script.Globals.Get("main");
        }

        public string Name { get; set; }

        public void Run()
        {
            script.Call(mainFunction);
        }

        public AddonConfiguration GetConfiguration()
        {
            return config;
        }

        public Table GetContext()
        {
            return script.Globals;
        }
    }
}