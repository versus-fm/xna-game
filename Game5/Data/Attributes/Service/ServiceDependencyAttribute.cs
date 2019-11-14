using System;

namespace Game5.Data.Attributes.Service
{
    public class ServiceDependencyAttribute : Attribute
    {
        public ServiceDependencyAttribute(Type dependency = null)
        {
            Dependency = dependency;
        }

        public Type Dependency { get; set; }
    }
}