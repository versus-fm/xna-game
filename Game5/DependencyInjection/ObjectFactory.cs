using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game5.Data.Attributes.DependencyInjection;
using Game5.Data.Attributes.Service;
using Game5.Data.Helper;
using Game5.Service;

namespace Game5.DependencyInjection
{
    /// <summary>
    ///     This class is for creating instances of objects requiring a service in the constructor, it will automatically pick
    ///     the first constructor, an explicitly defined constructor, or a constructor marked by
    ///     <see cref="FactoryConstructorAttribute" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectFactory<T>
    {
        private Type[] constructorInfo;
        private readonly Dictionary<Type, object> objectTypes;

        public ObjectFactory()
        {
            objectTypes = new Dictionary<Type, object>();
        }

        /// <summary>
        ///     Supplies the <see cref="ObjectFactory{T}" /> with the selected objects for the selected types.
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ObjectFactory<T> With(params (Type type, object impl)[] objects)
        {
            foreach (var obj in objects) objectTypes.Add(obj.type, obj.impl);
            return this;
        }

        /// <summary>
        ///     Explicitly define what constructor to use when making the object
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <returns></returns>
        public ObjectFactory<T> Using(params Type[] constructorInfo)
        {
            this.constructorInfo = constructorInfo;
            return this;
        }

        public T Make(params object[] additionalArgs)
        {
            var consumedArgs = 0;
            var type = typeof(T);
            var obj = default(T);
            if (constructorInfo != null)
            {
                var constructor = type.GetConstructor(constructorInfo);
                var paramObjects = constructorInfo.Select(x =>
                {
                    var service = TryGetService(x);
                    if (service == null)
                    {
                        service = additionalArgs[consumedArgs];
                        consumedArgs++;
                    }

                    return service;
                }).ToArray();
                obj = (T) constructor.Invoke(paramObjects);
            }
            else
            {
                var constructors = type.GetConstructors();
                var constructor =
                    constructors.FirstOrDefault(x => x.GetCustomAttribute<FactoryConstructorAttribute>() != null);
                if (constructor != null)
                {
                    var types = constructor.GetParameters();
                    var paramObjects = types.Select(x =>
                    {
                        var service = TryGetService(x.ParameterType);
                        if (service == null)
                        {
                            service = additionalArgs[consumedArgs];
                            consumedArgs++;
                        }

                        return service;
                    }).ToArray();
                    obj = (T) constructor.Invoke(paramObjects);
                }
                else
                {
                    constructor = type.GetConstructors()[0];
                    var types = constructor.GetParameters();
                    var paramObjects = types.Select(x =>
                    {
                        var service = TryGetService(x.ParameterType);
                        if (service == null)
                        {
                            service = additionalArgs[consumedArgs];
                            consumedArgs++;
                        }

                        return service;
                    }).ToArray();
                    obj = (T) constructor.Invoke(paramObjects);
                }
            }

            var dependencies = obj.GetType().GetPropertiesWithAttribute<ServiceDependencyAttribute>();
            foreach (var dependency in dependencies)
            {
                var attribute = dependency.GetCustomAttribute<ServiceDependencyAttribute>();
                var t = attribute.Dependency ?? dependency.PropertyType;
                dependency.SetValue(obj, TryGetService(t));
            }

            return obj;
        }

        private object TryGetService(Type type)
        {
            if (objectTypes.ContainsKey(type)) return objectTypes[type];

            if (ServiceLocator.HasService(type)) return ServiceLocator.Get(type);

            if (ServiceLocator.TryGetImplementationType(type).HasAttribute<ServiceAttribute>())
            {
                if (!ServiceLocator.HasService(type))
                    ServiceLocator.RegisterService(ServiceLocator.TryGetImplementationType(type));
                return ServiceLocator.Get(type);
            }

            return null;
        }
    }

    /// <summary>
    ///     This class is for creating instances of objects requiring a service in the constructor, it will automatically pick
    ///     the first constructor, an explicitly defined constructor, or a constructor marked by
    ///     <see cref="FactoryConstructorAttribute" />.
    ///     This is a non generic version of <see cref="ObjectFactory{T}" />
    /// </summary>
    public class ObjectFactory
    {
        private Type[] constructorInfo;
        private readonly Dictionary<Type, object> objectTypes;
        private readonly Type type;

        public ObjectFactory(Type type)
        {
            this.type = type;
            objectTypes = new Dictionary<Type, object>();
        }

        /// <summary>
        ///     Supplies the <see cref="ObjectFactory" /> with the selected objects for the selected types.
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public ObjectFactory With(params (Type type, object impl)[] objects)
        {
            foreach (var obj in objects) objectTypes.Add(obj.type, obj.impl);
            return this;
        }

        /// <summary>
        ///     Explicitly define what constructor to use when making the object
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <returns></returns>
        public ObjectFactory Using(params Type[] constructorInfo)
        {
            this.constructorInfo = constructorInfo;
            return this;
        }

        /// <summary>
        ///     Used for defining the values of args that are not services, in order.
        /// </summary>
        /// <param name="additionalArgs"></param>
        /// <returns></returns>
        public object Make(params object[] additionalArgs)
        {
            var consumedArgs = 0;
            object obj = null;
            if (constructorInfo != null)
            {
                var constructor = type.GetConstructor(constructorInfo);
                var paramObjects = constructorInfo.Select(x =>
                {
                    var service = TryGetService(x);
                    if (service == null)
                    {
                        service = additionalArgs[consumedArgs];
                        consumedArgs++;
                    }

                    return service;
                }).ToArray();
                obj = constructor.Invoke(paramObjects);
            }
            else
            {
                var constructors = type.GetConstructors();
                var constructor =
                    constructors.FirstOrDefault(x => x.GetCustomAttribute<FactoryConstructorAttribute>() != null);
                if (constructor != null)
                {
                    var types = constructor.GetParameters();
                    var paramObjects = types.Select(x =>
                    {
                        var service = TryGetService(x.ParameterType);
                        if (service == null)
                        {
                            service = additionalArgs[consumedArgs];
                            consumedArgs++;
                        }

                        return service;
                    }).ToArray();
                    obj = constructor.Invoke(paramObjects);
                }
                else
                {
                    constructor = type.GetConstructors()[0];
                    var types = constructor.GetParameters();
                    var paramObjects = types.Select(x =>
                    {
                        var service = TryGetService(x.ParameterType);
                        if (service == null)
                        {
                            service = additionalArgs[consumedArgs];
                            consumedArgs++;
                        }

                        return service;
                    }).ToArray();
                    obj = constructor.Invoke(paramObjects);
                }
            }

            var dependencies = obj.GetType().GetPropertiesWithAttribute<ServiceDependencyAttribute>();
            foreach (var dependency in dependencies)
            {
                var attribute = dependency.GetCustomAttribute<ServiceDependencyAttribute>();
                var t = attribute.Dependency ?? dependency.PropertyType;
                dependency.SetValue(obj, TryGetService(t));
            }

            return obj;
        }

        private object TryGetService(Type type)
        {
            if (objectTypes.ContainsKey(type)) return objectTypes[type];

            if (ServiceLocator.HasService(type)) return ServiceLocator.Get(type);

            if (ServiceLocator.TryGetImplementationType(type).HasAttribute<ServiceAttribute>())
            {
                if (!ServiceLocator.HasService(type))
                    ServiceLocator.RegisterService(ServiceLocator.TryGetImplementationType(type));
                return ServiceLocator.Get(type);
            }

            return null;
        }
    }
}