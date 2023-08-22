using System;
using System.Threading.Tasks;
using AHL.Core.Events;
using AHL.Core.General.Utils;
using AHL.Core.Main;

namespace AHL.Core.General
{
    public class AHLBaseManager
    {
        protected readonly AHLManager AHLManager;
        private readonly Action<AHLBaseManager> completeLoadAction;
        
        protected void InvokeEvent(IAHLEvent ahlEvent) => AHLManager?.Events?.InvokeAHLEvent(ahlEvent);
        
        protected void AddListener<T>(Action<T> action, int priority = 100) where T : IAHLEvent => 
            AHLManager.Events.AddEventListener(action, priority);
		
        protected void RemoveListener<T>(Action<T> action) where T : IAHLEvent => 
            AHLManager.Events.RemoveEventListener(action);
        
        protected bool isInit;
        
        public AHLBaseManager(AHLManager manager, Action<AHLBaseManager> onComplete)
        {
#if MANAGER_FLOW_LOG_ENABLED
            AHLDebug.Log($"#AHLBaseManager# {GetType().FullName} Action");
#endif
            this.AHLManager = manager;
            completeLoadAction = onComplete;
        }
        
        ~AHLBaseManager()
        {
            
        }

        protected virtual async void OnInitComplete()
        {
#if MANAGER_FLOW_LOG_ENABLED
            AHLDebug.Log($"#AHLBaseManager# {GetType().FullName} OnInitComplete");
#endif

            isInit = true;
            
            completeLoadAction?.Invoke(this);
        }

        public virtual void ReleaseManager(Action action)
        {
            AHLDebug.Log($"#AHLBaseManager# {GetType().FullName} Released");
            action?.Invoke();
        }
    }
}