using System.Collections.Generic;
using System.Linq;
using Game5.Data.Attributes.UI;
using Newtonsoft.Json.Linq;

namespace Game5.UI.Styling
{
    public class InterfaceStyle
    {
        public Dictionary<string, ControlStyle> styles;

        private InterfaceStyle()
        {
            styles = new Dictionary<string, ControlStyle>();
        }

        public ControlStyle GetStyleFor(UserControl control)
        {
            var attr = (UIDocElementAttribute) control.GetType()
                .GetCustomAttributes(typeof(UIDocElementAttribute), false).First();
            if (attr != null && styles.ContainsKey(attr.ElementName)) return styles[attr.ElementName];
            return new ControlStyle();
        }

        public ControlStyle GetStyleFor(string name)
        {
            if (styles.ContainsKey(name)) return styles[name];
            return new ControlStyle();
        }

        public static InterfaceStyle CreateFrom(string json)
        {
            var interfaceStyle = new InterfaceStyle();
            var obj = JObject.Parse(json);
            foreach (var root in obj.Root)
            {
                var control = new ControlStyle();
                foreach (JProperty val in root.Values()) control.Set(val.Name, (string) val.Value);
                interfaceStyle.styles.Add(root.Path, control);
            }

            return interfaceStyle;
        }
    }
}