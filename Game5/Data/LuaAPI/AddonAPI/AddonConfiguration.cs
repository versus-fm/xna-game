using System.Collections.Generic;

namespace Game5.Data.LuaAPI.AddonAPI
{
    public class AddonConfiguration
    {
        public string Author { get; set; } = "";
        public string Version { get; set; } = "";
        public string MainFunction { get; set; } = "";
        public bool OpenAccess { get; set; } = false;
        public List<string> Dependencies { get; set; } = new List<string>();
        public List<string> Modules { get; set; } = new List<string>();
    }
}