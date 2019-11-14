using System.Collections.Generic;
using Game5.Data.Attributes.Lua;
using Game5.Data.LuaAPI.AddonAPI;
using MoonSharp.Interpreter;

namespace Game5.Data.LuaAPI.API.Statics
{
    [LuaStaticClass("addons")]
    public class LuaAddon
    {
        private static Dictionary<(string addon, string module), Table> modules;

        public static Table GetAddonContext(string name)
        {
            var addon = LuaContext.GetAddon(name);
            if (addon.GetConfiguration().OpenAccess)
                return LuaContext.GetAddon(name).GetContext();
            return null;
        }

        public static void ExportTable(string addon, string tableName, Table table)
        {
            if (modules == null) modules = new Dictionary<(string addon, string module), Table>();
            modules.Add((addon, tableName), table);
        }

        public static Table ImportTable(string addon, string table)
        {
            if (modules == null) modules = new Dictionary<(string addon, string module), Table>();
            if (!modules.ContainsKey((addon, table)))
                return null;
            return modules[(addon, table)];
        }
    }
}