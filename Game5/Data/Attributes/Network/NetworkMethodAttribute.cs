using System;

namespace Game5.Data.Attributes.Network
{
    public class NetworkMethodAttribute : Attribute
    {
        public NetworkMethodAttribute(short id)
        {
            Id = id;
        }

        public short Id { get; set; }
    }
}