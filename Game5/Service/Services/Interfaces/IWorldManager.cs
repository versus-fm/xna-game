using Game5.Env.World;

namespace Game5.Service.Services.Interfaces
{
    public interface IWorldManager
    {
        IWorld AddWorld(IWorldProvider provider);
        void DisposeWorld(byte id);
        IWorld GetWorld(byte id);
    }
}