using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5.Service.Services.Interfaces
{
    public interface IEventHandler
    {
        void FireEvent(string name, params object[] args);
        void SubscribeEvent(string name, Action<object[]> eventAction);
    }
}
