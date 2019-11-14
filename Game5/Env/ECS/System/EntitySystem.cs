using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5.Env.ECS.System
{
    public abstract class EntitySystem
    {
        protected List<ushort> entities;
        public EntitySystem()
        {
            entities = new List<ushort>();
        }
        public abstract void Draw(ushort entity);
        public abstract void Update(ushort entity);
        public void OnEntityAdded(ushort entity)
        {
            entities.Add(entity);
        }
        public void OnEntityRemoved(ushort entity)
        {
            entities.Remove(entity);
        }
        public abstract string[] GetRequiredComponents();
    }
}
