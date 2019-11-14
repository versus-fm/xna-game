using System.Collections.Generic;

namespace Game5.Resource
{
    public class Spritesheet
    {
        public string path { get; set; }
        public string name { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<Alias> Aliases { get; set; }
    }
}