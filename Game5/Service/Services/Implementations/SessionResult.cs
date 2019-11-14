using System;
using System.Collections.Generic;

namespace Game5.Service.Services.Implementations
{
    public struct SessionResult
    {
        public Dictionary<string, TimeSpan> Components;
        public TimeSpan TotalTime;

        public TimeSpan TryGetComponent(string name)
        {
            if (!Components.ContainsKey(name)) return new TimeSpan();
            return Components[name];
        }
    }
}