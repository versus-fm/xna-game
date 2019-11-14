using Game5.Env;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Game5.Data.Helper
{
    public static class ObjectHelper
    {
        public static T ChangeType<T>(this object value, CultureInfo cultureInfo)
        {
            var toType = typeof(T);

            if (value == null) return default(T);

            if (value is string)
            {
                if (toType == typeof(Guid))
                    return ChangeType<T>(new Guid(Convert.ToString(value, cultureInfo)), cultureInfo);
                if ((string) value == string.Empty && toType != typeof(string)) return ChangeType<T>(null, cultureInfo);
            }
            else
            {
                if (typeof(T) == typeof(string))
                    return ChangeType<T>(Convert.ToString(value, cultureInfo), cultureInfo);
            }

            if (toType.IsGenericType &&
                toType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                toType = Nullable.GetUnderlyingType(toType);
                ;
            }

            var canConvert = toType is IConvertible || toType.IsValueType && !toType.IsEnum;
            if (canConvert) return (T) Convert.ChangeType(value, toType, cultureInfo);
            return (T) value;
        }

        public static object ChangeType(this object value, Type toType, CultureInfo cultureInfo)
        {
            if (value == null) return null;

            if (value is string)
            {
                if (toType == typeof(Guid))
                    return ChangeType<Guid>(new Guid(Convert.ToString(value, cultureInfo)), cultureInfo);
                if ((string) value == string.Empty && toType != typeof(string)) return null;
            }
            else
            {
                if (toType == typeof(string))
                    return ChangeType<string>(Convert.ToString(value, cultureInfo), cultureInfo);
            }

            if (toType.IsGenericType &&
                toType.GetGenericTypeDefinition() == typeof(Nullable<>))
                toType = Nullable.GetUnderlyingType(toType);

            var canConvert = toType is IConvertible || toType.IsValueType && !toType.IsEnum;
            if (canConvert) return Convert.ChangeType(value, toType, cultureInfo);
            return value;
        }

        public static bool HasAttribute<T>(this PropertyInfo obj) where T : Attribute
        {
            return obj.GetCustomAttribute<T>() != null;
        }

        public static bool HasAttribute(this PropertyInfo obj, Type attribute)
        {
            if (!attribute.IsSubclassOf(typeof(Attribute))) throw new ArgumentException("Illegal argument");
            return obj.GetCustomAttribute(attribute) != null;
        }

        public static bool HasAttribute<T>(this MethodInfo obj) where T : Attribute
        {
            return obj.GetCustomAttribute<T>() != null;
        }

        public static bool HasAttribute(this MethodInfo obj, Type attribute)
        {
            if (!attribute.IsSubclassOf(typeof(Attribute))) throw new ArgumentException("Illegal argument");
            return obj.GetCustomAttribute(attribute) != null;
        }

        public static bool HasAttribute<T>(this Type obj) where T : Attribute
        {
            return obj.GetCustomAttribute<T>() != null;
        }

        public static bool HasAttribute(this Type obj, Type attribute)
        {
            if (!attribute.IsSubclassOf(typeof(Attribute))) throw new ArgumentException("Illegal argument");
            return obj.GetCustomAttribute(attribute) != null;
        }

        public static List<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type) where T : Attribute
        {
            var properties = new List<PropertyInfo>();
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic |
                                                        BindingFlags.Public))
                if (property.HasAttribute<T>())
                    properties.Add(property);
            return properties;
        }

        public static List<PropertyInfo> GetPropertiesWithAttribute(this Type type, Type attribute)
        {
            var properties = new List<PropertyInfo>();
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic |
                                                        BindingFlags.Public))
                if (property.HasAttribute(attribute))
                    properties.Add(property);
            return properties;
        }

        public static List<MethodInfo> GetMethodsWithAttribute<T>(this Type type) where T : Attribute
        {
            var methods = new List<MethodInfo>();
            foreach (var method in type.GetMethods())
                if (method.HasAttribute<T>())
                    methods.Add(method);
            return methods;
        }

        public static List<MethodInfo> GetMethodsWithAttribute(this Type type, Type attribute)
        {
            var methods = new List<MethodInfo>();
            foreach (var method in type.GetMethods())
                if (method.HasAttribute(attribute))
                    methods.Add(method);
            return methods;
        }
    }
}