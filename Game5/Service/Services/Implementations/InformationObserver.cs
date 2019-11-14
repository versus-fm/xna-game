using Game5.Data.Attributes.Service;
using Game5.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IInformationObserver))]
    public class InformationObserver : IInformationObserver
    {
        private Dictionary<string, Func<object[], object>> providerFunctions;

        public InformationObserver()
        {
            providerFunctions = new Dictionary<string, Func<object[], object>>();
        }

        public void Provide(string name, Func<object[], object> provider)
        {
            if(providerFunctions.ContainsKey(name))
            {
                return;
            }
            providerFunctions.Add(name, provider);
        }

        public object Retrieve(string name, params object[] args)
        {
            if(!providerFunctions.ContainsKey(name))
            {
                return null;
            }
            return providerFunctions[name](args);
        }
    }
}
