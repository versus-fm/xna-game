using System;
using System.Linq.Expressions;
using MoonSharp.Interpreter.Interop;

namespace Game5.Data.LuaAPI.AddonAPI.Factory
{
    public class ProxyFactory : IProxyFactory
    {
        private readonly Constructor constructor;

        public ProxyFactory(Type target, Type proxy)
        {
            TargetType = target;
            ProxyType = proxy;

            var constructor = ProxyType.GetConstructor(new[] {typeof(object)});
            var param = Expression.Parameter(typeof(object), "args");
            var newExp = Expression.New(constructor, param);
            var lambda = Expression.Lambda(typeof(Constructor), newExp, param);
            this.constructor = (Constructor) lambda.Compile();
        }

        public Type TargetType { get; }

        public Type ProxyType { get; }

        public object CreateProxyObject(object o)
        {
            return constructor(o);
        }

        private delegate object Constructor(object arg);
    }
}