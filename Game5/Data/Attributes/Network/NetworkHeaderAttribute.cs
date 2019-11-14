using System;

namespace Game5.Data.Attributes.Network
{
    public class NetworkHeaderAttribute : Attribute
    {
        public NetworkHeaderAttribute(HeaderType type)
        {
            HeaderType = type;
        }

        public HeaderType HeaderType { get; set; }
    }
}