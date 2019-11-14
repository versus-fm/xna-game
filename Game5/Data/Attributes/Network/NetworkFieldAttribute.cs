namespace Game5.Data.Attributes.Network
{
    public class NetworkFieldAttribute
    {
        public NetworkFieldAttribute(short id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}