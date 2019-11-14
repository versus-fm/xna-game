using Game5.Data.Attributes.Service;
using Game5.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IEventHandler))]
    public class EventHandler : IEventHandler
    {
        private Dictionary<string, List<Action<object[]>>> events;
        public EventHandler()
        {
            events = new Dictionary<string, List<Action<object[]>>>();
        }

        public void FireEvent(string name, params object[] args)
        {
            if(events.ContainsKey(name))
            {
                events[name].ForEach(x => x(args));
            }
        }

        public void SubscribeEvent(string name, Action<object[]> eventAction)
        {
            if (!events.ContainsKey(name)) events.Add(name, new List<Action<object[]>>());
            events[name].Add(eventAction);
        }
    }
}
