using System;

namespace Game5.Service
{
    public class ServiceImplementationNotFoundException : Exception
    {
        public ServiceImplementationNotFoundException(Type service) : base(
            "Implementation not found for service of type " + service.FullName)
        {
            ServiceType = service;
        }

        public Type ServiceType { get; set; }
    }
}