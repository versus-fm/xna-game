using System.Collections;
using System.Collections.Generic;

namespace Game5.UI.Styling
{
    public class ControlStyle : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> styledProperties;

        public ControlStyle()
        {
            styledProperties = new Dictionary<string, string>();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>) styledProperties).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>) styledProperties).GetEnumerator();
        }

        public string TryGet(string propertyName)
        {
            if (!styledProperties.ContainsKey(propertyName)) return "";
            return styledProperties[propertyName];
        }

        public void Set(string propertyName, string value)
        {
            styledProperties.Add(propertyName, value);
        }
    }
}