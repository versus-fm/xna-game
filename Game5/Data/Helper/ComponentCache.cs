using System;
using System.Collections.Generic;
using Game5.Data.Attributes;
using Game5.Data.Attributes.Component;

namespace Game5.Data.Helper
{
    internal static class ComponentCache
    {
        private static Dictionary<string, Type> components;

        private static void Construct()
        {
            if (components == null) components = new Dictionary<string, Type>();
        }

        public static void RegisterComponents()
        {
            Construct();
            var domain = AppDomain.CurrentDomain;
            var assemblies = domain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = TypeHelper.FindAllTypesWithAttribute<ComponentAttribute>();
                foreach (var type in types)
                    if (!components.ContainsKey(type.attribute.Name))
                    {
                        var name = type.attribute.Name;
                        if (string.IsNullOrWhiteSpace(name)) name = type.type.Name.ToSnakeCase();
                        components.Add(type.attribute.Name, type.type);
                    }
            }
        }

        public static Type GetComponent(string name)
        {
            return components[name];
        }
    }
}