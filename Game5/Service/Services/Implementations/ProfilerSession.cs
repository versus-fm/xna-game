using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game5.Service.Services.Implementations
{
    public class ProfilerSession
    {
        private readonly bool allowComponents;
        private readonly Dictionary<string, TimeSpan> components;
        private TimeSpan totalTime;
        private readonly Stack<Stopwatch> watches;

        public ProfilerSession(bool allowComponents)
        {
            this.allowComponents = allowComponents;
            totalTime = new TimeSpan();
            watches = new Stack<Stopwatch>();
            components = new Dictionary<string, TimeSpan>();
        }

        public void Start()
        {
            watches.Push(Stopwatch.StartNew());
        }

        public void StartComponent()
        {
            if (!allowComponents) return;
            watches.Push(Stopwatch.StartNew());
        }

        public void EndComponent(string name)
        {
            if (!allowComponents) return;
            if (watches.Count <= 1) return;
            var sw = watches.Pop();
            sw.Stop();
            if (!components.ContainsKey(name))
                components.Add(name, sw.Elapsed);
            else
                components[name] += sw.Elapsed;
        }

        public SessionResult End()
        {
            var sw = watches.Pop();
            sw.Stop();
            totalTime += sw.Elapsed;
            var sr = new SessionResult();
            sr.Components = components;
            sr.TotalTime = totalTime;
            return sr;
        }
    }
}