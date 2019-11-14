using System;
using System.Collections.Generic;

namespace Game5.Data.Helper
{
    public static class TypeCache
    {
        private static Dictionary<string, Type> cache;
        private static readonly object cacheLock = new object();

        public static Type GetType(string assemblyQualifiedName)
        {
            lock (cacheLock)
            {
                if (cache == null) cache = new Dictionary<string, Type>();
                if (cache.ContainsKey(assemblyQualifiedName))
                {
                    return cache[assemblyQualifiedName];
                }

                var type = Type.GetType(assemblyQualifiedName);
                cache.Add(assemblyQualifiedName, type);
                return type;
            }
        }

        public static void MakeDirty()
        {
            cache = new Dictionary<string, Type>();
        }
    }
}