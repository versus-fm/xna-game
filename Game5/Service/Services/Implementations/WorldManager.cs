using Game5.Data.Attributes.DependencyInjection;
using Game5.Data.Attributes.Service;
using Game5.Env.World;
using Game5.Service.Services.Interfaces;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IWorldManager))]
    public class WorldManager : IWorldManager
    {
        private IPropertyService propertyService;

        private readonly IWorld[] worlds;

        [FactoryConstructor]
        public WorldManager(IPropertyService propertyService)
        {
            worlds = new IWorld[byte.MaxValue];
        }

        public IWorld AddWorld(IWorldProvider provider)
        {
            var id = GetFreeID();
            IWorld world = null;

            world.SetID(id);
            worlds[id] = world;
            return world;
        }

        public IWorld GetWorld(byte id)
        {
            return worlds[id];
        }

        public void DisposeWorld(byte id)
        {
        }

        private byte GetFreeID()
        {
            byte id = 0;
            while (worlds[id] != null) id++;
            return id;
        }
    }
}