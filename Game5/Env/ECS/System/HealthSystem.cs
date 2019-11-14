using Game5.Data.Attributes.Service;
using Game5.Env.ECS.Component;
using Game5.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5.Env.ECS.System
{
    public class HealthSystem : EntitySystem
    {
        private static readonly string[] components = { "health_component" };
        [ServiceDependency]
        public IStateService StateService { get; set; }
        [ServiceDependency]
        public IInformationObserver Observer { get; set; }
        [ServiceDependency]
        public IEventHandler Events { get; set; }
        public HealthSystem()
        {

        }

        public void Draw(ushort id)
        {
        }

        public string[] GetRequiredComponents()
        {
            return components;
        }

        public void Update(ushort id)
        {
            var health = entity.GetComponent("health_component");

        }

        public void OnEntityAdded(ushort id)
        {
            throw new NotImplementedException();
        }

        public void OnEntityRemoved(ushort id)
        {
            throw new NotImplementedException();
        }
    }
}
