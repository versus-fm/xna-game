using System;

namespace Game5.Data.Attributes.Service
{
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute(Type serviceDefinition = null)
        {
            ServiceDefinition = serviceDefinition;
        }

        public Type ServiceDefinition { get; set; }
    }
}