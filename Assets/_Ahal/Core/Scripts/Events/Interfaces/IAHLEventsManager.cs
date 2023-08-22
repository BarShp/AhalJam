using System;

namespace AHL.Core.Events
{
    public interface IAHLEventsManager
    {
        void InvokeAHLEvent(IAHLEvent evt);
        void AddEventListener<T>(Action<T> action, int priority = 100) where T : IAHLEvent;
        void RemoveEventListener<T>(Action<T> action) where T : IAHLEvent;
        void ReleaseManager(Action action);
    }
}