// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceLocator.cs" company="">
//   
// </copyright>
// <summary>
//   Extension class for
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Game5.Data.Attributes.Service;
using Game5.Data.Helper;
using Game5.DependencyInjection;

namespace Game5.Service
{
    /// <summary>
    ///     ServiceLocator class
    /// </summary>
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> services;
        private static Dictionary<Type, Type> implementations;

        public static void RegisterImplementations()
        {
            if (implementations == null) implementations = new Dictionary<Type, Type>();
            if (services == null) services = new Dictionary<Type, object>();
            var types = TypeHelper.FindAllTypesWithAttribute<ServiceAttribute>();
            foreach (var type in types)
                if (!implementations.ContainsKey(type.attribute.ServiceDefinition))
                    implementations.Add(type.attribute.ServiceDefinition, type.type);
        }

        /// <summary>
        ///     Use this to overwrite any implementations found by <see cref="RegisterImplementations" />
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="implementation"></param>
        public static void OverwriteImplementation(Type definition, Type implementation)
        {
            if (implementations.ContainsKey(definition)) implementations[definition] = implementation;
            else implementations.Add(definition, implementation);
        }

        public static void RegisterServices()
        {
            foreach (var type in implementations) RegisterService(type.Value, true);
        }

        public static void RegisterService<T>(T service, bool nice = false)
        {
            if (services == null) services = new Dictionary<Type, object>();
            if (service.GetType().HasAttribute<ServiceAttribute>())
            {
                var serviceAttribute = service.GetType().GetCustomAttribute<ServiceAttribute>();
                var keyType = serviceAttribute.ServiceDefinition ?? typeof(T);
                if (services.ContainsKey(keyType))
                {
                    if (!nice) services[keyType] = service;
                }
                else
                {
                    services.Add(keyType, service);
                }
            }
            else
            {
                if (services.ContainsKey(typeof(T)))
                {
                    if (!nice) services[typeof(T)] = service;
                }
                else
                {
                    services.Add(typeof(T), service);
                }
            }

            InjectDependencies(service);
        }

        public static void RegisterService<T>(bool nice = false)
        {
            RegisterService(typeof(T), nice);
        }

        public static void RegisterService(Type type, bool nice = false)
        {
            if (services == null) services = new Dictionary<Type, object>();
            object service = null;
            if (type.HasAttribute<ServiceAttribute>())
            {
                var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();
                var keyType = serviceAttribute.ServiceDefinition ?? type;
                service = new ObjectFactory(type).Make();
                if (services.ContainsKey(keyType))
                {
                    if (!nice) services[keyType] = service;
                }
                else
                {
                    services.Add(keyType, service);
                }
            }
            else
            {
                service = new ObjectFactory(type).Make();
                if (services.ContainsKey(type))
                {
                    if (!nice) services[type] = service;
                }
                else
                {
                    services.Add(type, service);
                }
            }

            InjectDependencies(service);
        }

        public static void InjectDependencies(object target)
        {
            var dependencies = target.GetType().GetPropertiesWithAttribute<ServiceDependencyAttribute>();
            foreach (var dependency in dependencies)
            {
                var attribute = dependency.GetCustomAttribute<ServiceDependencyAttribute>();
                var t = attribute.Dependency ?? dependency.PropertyType;
                if (!services.ContainsKey(t)) RegisterService(t);
                dependency.SetValue(target, services[t]);
            }
        }

        public static bool HasService(Type service)
        {
            return services.ContainsKey(service);
        }

        public static bool HasService<T>()
        {
            return HasService(typeof(T));
        }

        public static T Get<T>()
        {
            return (T) Get(typeof(T));
        }

        public static object Get(Type type)
        {
            if (!services.ContainsKey(type)) throw new ServiceImplementationNotFoundException(type);
            return services[type];
        }

        public static Type TryGetImplementationType(Type type)
        {
            if (implementations.ContainsKey(type))
                return implementations[type];
            return type;
        }
    }
}