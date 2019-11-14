using Game5.Service.Services.Implementations;

namespace Game5.Service.Services.Interfaces
{
    public interface IPerformanceProfiler
    {
        ProfilerSession Active { get; }

        void StartSession(bool allowComponents);
        SessionResult StopSession();
    }
}