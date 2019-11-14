using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Game5.DependencyInjection;
using Game5.Env.ECS.System;

namespace Game5.Env.ECS
{
    public class EntitySceneManager
    {
        private const int MAX_ENTITIES = 256;
        private List<EntitySystem> systems;
        private Dictionary<string, List<ushort>> components;
        private Dictionary<string, List<ushort>> tags;
        private ushort emptyId;

        public EntitySceneManager()
        {
            systems = new List<EntitySystem>();
            components = new Dictionary<string, List<ushort>>();
            tags = new Dictionary<string, List<ushort>>();
            emptyId = 0;
        }

        public void Update()
        {
            
        }

        public void Draw()
        {
            
        }

        public ushort AddEntity()
        {
            if (emptyId == ushort.MaxValue - 1) return ushort.MaxValue;
            var id = EmptyId();
            return id;
        }

        public void DeleteEntity(ushort id)
        {

        }

        public void AddSystem<T>(params object[] args) where T : EntitySystem
        {
            var systemFactory = new ObjectFactory<T>();
            var system = systemFactory.Make(args);
            systems.Add(system);
        }

        private ushort EmptyId()
        {
            while (entities[emptyId])
                emptyId++;
            return emptyId;
        }

    }
}
