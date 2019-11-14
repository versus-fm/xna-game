using Game5.Data.Attributes.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5.Env.ECS.Component
{
    [Component]
    public class HealthComponent : IEntityComponent
    {
        private float health;
        public HealthComponent()
        {

        }

        public float Health { get => health; set => health = value; }
    }
}
