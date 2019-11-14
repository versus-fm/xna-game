using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game5.Data.Helper
{
    public static class TypeHelper
    {
        private static List<string> invalidAssemblies;

        public static List<(T attribute, Type type)> FindAllTypesWithAttribute<T>(Assembly assembly) where T : Attribute
        {
            if (invalidAssemblies == null) invalidAssemblies = new List<string>();
            var list = new List<(T, Type)>();
            if (invalidAssemblies.Contains(assembly.FullName)) return list;
            try
            {
                foreach (var type in assembly.GetTypes())
                    if (type.GetCustomAttributes(typeof(T), true).Length > 0)
                        list.Add((type.GetCustomAttribute<T>(), type));
            }
            catch (Exception e)
            {
                if (e is ReflectionTypeLoadException)
                {
                    invalidAssemblies.Add(assembly.FullName);
                    var typeLoadException = e as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                    foreach (var ex in loaderExceptions) Console.WriteLine(ex.Message);
                }
            }

            return list;
        }

        public static List<(T attribute, Type type)> FindAllTypesWithAttribute<T>() where T : Attribute
        {
            var list = new List<(T attribute, Type type)>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies) list.AddRange(FindAllTypesWithAttribute<T>(assembly));
            return list;
        }

        //public static byte[] WriteProperties(object target)
        //{
        //    var bytes = new byte[1024];
        //    var type = target.GetType();
        //    var pos = 0;
        //    foreach (var property in type.GetProperties())
        //    {
        //        if (StorageMethod.IsSimple(property.PropertyType))
        //        {
        //            var b = StorageMethod.PerformGet(property.PropertyType, property.GetValue(target));
        //            bytes = bytes.Append(pos, b);
        //            pos += b.Length;
        //        }
        //    }
        //    return bytes;
        //}
        //public static byte[] WriteProperties(ref byte[] bytes, int position, object target)
        //{
        //    var type = target.GetType();
        //    var pos = position;
        //    foreach (var property in type.GetProperties())
        //    {
        //        if (StorageMethod.IsSimple(property.PropertyType))
        //        {
        //            var b = StorageMethod.PerformGet(property.PropertyType, property.GetValue(target));
        //            bytes = bytes.Append(pos, b);
        //            pos += b.Length;
        //        }
        //    }
        //    return bytes;
        //}
        //public static void ReadProperties(byte[] bytes, object target)
        //{
        //    var type = target.GetType();
        //    using (BinaryReader reader = new BinaryReader(new MemoryStream(bytes)))
        //    {
        //        foreach (var property in type.GetProperties())
        //        {
        //            if (StorageMethod.IsSimple(property.PropertyType))
        //            {
        //                property.SetValue(target, StorageMethod.PerformSet(property.PropertyType, reader));
        //            }
        //        }
        //    }
        //}
        //public static void ReadProperties(BinaryReader reader, object target)
        //{
        //    var type = target.GetType();
        //    foreach (var property in type.GetProperties())
        //    {
        //        if (StorageMethod.IsSimple(property.PropertyType))
        //        {
        //            property.SetValue(target, StorageMethod.PerformSet(property.PropertyType, reader));
        //        }
        //    }
        //}
    }
}