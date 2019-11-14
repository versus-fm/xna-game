using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game5.Data.Attributes.Lua;
using Game5.Data.Helper;
using Game5.Data.LuaAPI.AddonAPI.Factory;
using MoonSharp.Interpreter;
using Newtonsoft.Json;

namespace Game5.Data.LuaAPI.AddonAPI
{
    public class LuaContext
    {
        private static Dictionary<string, Addon> addons;
        private static Dictionary<string, Type> statics;
        private static Dictionary<string, List<Closure>> actions;
        private static Script globalContext;

        public static void LoadAddons()
        {
            SetupEnvironment();
            actions = new Dictionary<string, List<Closure>>();
            var directories =
                Directory.GetDirectories(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "addons");
            foreach (var directory in directories)
            {
                var addonName = Path.GetFileNameWithoutExtension(directory);
                var configFile = directory + Path.DirectorySeparatorChar + addonName + ".json";
                var mainLua = directory + Path.DirectorySeparatorChar + addonName + ".lua";
                var addonScript = new Script(CoreModules.Preset_SoftSandbox);

                var config = JsonConvert.DeserializeObject<AddonConfiguration>(File.ReadAllText(configFile));
                addonScript.DoFile(mainLua);

                foreach (var stat in statics)
                    if (config.Modules.Contains(stat.Key))
                        addonScript.Globals[stat.Key] = stat.Value;

                addonScript.Globals.Set("ADDON_NAME", DynValue.NewString(addonName));
                addonScript.Globals.Set("ADDON_AUTHOR", DynValue.NewString(config.Author));
                addonScript.Globals.Set("ADDON_VERSION", DynValue.NewString(config.Version));
                addonScript.Globals.Set("ADDON_MAIN", DynValue.NewString(config.MainFunction));
                addonScript.Globals.Set("ADDON_OPEN", DynValue.NewBoolean(config.OpenAccess));
                addonScript.Globals.Set("ADDON_PATH", DynValue.NewString(directory));

                var addon = new Addon(addonName, addonScript, config);
                addons.Add(addonName, addon);
            }

            var addonList = addons.Values.ToList();
            var loadedAddons = new List<string>();
            for (var i = 0; i < addonList.Count; i++)
            {
                var addon = addonList[i];
                if (addon.GetConfiguration().Dependencies.TrueForAll(x => loadedAddons.Contains(x)))
                {
                    addon.Run();
                    loadedAddons.Add(addon.Name);
                }
                else if (addon.GetConfiguration().Dependencies
                    .TrueForAll(x => addonList.Select(y => y.Name).Contains(x)))
                {
                    addonList.Add(addon);
                }
                else
                {
                    Console.WriteLine("Dependency Missing for " + addon.Name);
                }
            }
        }

        public static Addon GetAddon(string addon)
        {
            return addons[addon];
        }

        public static void FireEvent(string name, params object[] args)
        {
            if (actions.ContainsKey(name))
            {
                if (args.Length == 0)
                    actions[name].ForEach(x => x.Call());
                else
                    actions[name].ForEach(x => x.Call(args));
            }
        }

        public static void Subscribe(string name, Closure d)
        {
            if (actions.ContainsKey(name))
                actions[name].Add(d);
            else
                actions.Add(name, new List<Closure> {d});
        }

        /// <summary>
        ///     For running simple code outside of the addon system on a single global context. The global table includes all
        ///     static classes. Beware of using non local values in the lua code here.
        /// </summary>
        /// <param name="s">The code to run</param>
        public static DynValue DoCode(string s)
        {
            return globalContext.DoString(s);
        }

        public static DynValue LoadCode(string s)
        {
            return globalContext.LoadString(s);
        }

        public static DynValue DoFunction(DynValue val)
        {
            return globalContext.Call(val);
        }

        private static void SetupEnvironment()
        {
            globalContext = new Script(CoreModules.Preset_SoftSandbox);
            addons = new Dictionary<string, Addon>();
            statics = new Dictionary<string, Type>();
            var proxytypes = TypeHelper.FindAllTypesWithAttribute<LuaProxyClassAttribute>();
            foreach (var proxy in proxytypes)
                UserData.RegisterProxyType(new ProxyFactory(proxy.attribute.Target, proxy.type));
            var statictypes = TypeHelper.FindAllTypesWithAttribute<LuaStaticClassAttribute>();
            foreach (var stat in statictypes)
            {
                UserData.RegisterType(stat.type);
                statics.Add(stat.attribute.Name, stat.type);
                globalContext.Globals[stat.attribute.Name] = stat.type;
            }
        }
    }
}