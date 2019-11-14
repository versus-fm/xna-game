using System.Collections.Generic;
using Game5.Data.Attributes.Service;
using Game5.Service.Services.Interfaces;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IPerformanceProfiler))]
    public class PerformanceProfiler : IPerformanceProfiler
    {
        private readonly Stack<ProfilerSession> timeStack;

        public PerformanceProfiler()
        {
            timeStack = new Stack<ProfilerSession>();
        }

        public ProfilerSession Active => timeStack.Peek();

        public void StartSession(bool allowComponents)
        {
            var session = new ProfilerSession(allowComponents);
            session.Start();
            timeStack.Push(session);
        }

        public SessionResult StopSession()
        {
            var sw = timeStack.Pop();
            return sw.End();
        }
    }
}