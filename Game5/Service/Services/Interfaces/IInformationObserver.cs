using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game5.Service.Services.Interfaces
{
    public interface IInformationObserver
    {
        object Retrieve(string name, params object[] args);
        void Provide(string name, Func<object[], object> provider);
    }
}
