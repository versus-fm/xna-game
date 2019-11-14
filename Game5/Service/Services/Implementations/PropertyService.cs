using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Game5.Data.Attributes.Service;
using Game5.Data.Helper;
using Game5.Service.Services.Interfaces;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IPropertyService))]
    public class PropertyService : IPropertyService
    {
        private readonly Dictionary<string, string> properties;

        public PropertyService()
        {
            properties = new Dictionary<string, string>();
        }

        public T GetProperty<T>(string key, T defaultValue = default(T))
        {
            if (!properties.ContainsKey(key)) properties.Add(key, defaultValue.ToString());
            return properties[key].ChangeType<T>(CultureInfo.CurrentCulture);
        }

        public void Load(string file)
        {
            var filepath = "cfg/" + file + ".properties";
            if (!File.Exists(filepath))
            {
                var fs = File.Create(filepath);
                fs.Dispose();
            }

            var lines = File.ReadAllLines(filepath);
            foreach (var line in lines)
            {
                var split = line.Split('=');
                var key = split[0];
                var value = split[1];
                properties.Add(key, value);
            }
        }

        public void Save(string file)
        {
            var filepath = "cfg/" + file + ".properties";

            var lines = new List<string>();
            foreach (var property in properties) lines.Add(property.Key + "=" + property.Value);
            File.WriteAllLines(filepath, lines);
        }

        public void SetProperty<T>(string key, T property)
        {
            if (!properties.ContainsKey(key))
                properties.Add(key, property.ToString());
            else
                properties[key] = property.ToString();
        }
    }
}