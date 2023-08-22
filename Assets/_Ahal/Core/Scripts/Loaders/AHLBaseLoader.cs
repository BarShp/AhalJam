using System;
using Microsoft.Collections.Extensions;
using AHL.Core.General;
using AHL.Core.Main;

namespace AHL.Core.Loaders
{
    public class AHLBaseLoader : AHLMonoBehaviour
    {
        private Action onLoadComplete;
        private Action onUnloadComplete;
        protected Action onLoaderStep;
        
        // TODO: Should be OrderedDictionary instead
        protected OrderedDictionary<Type, Action<AHLBaseManager>> ManagerLoadingOrder;

        public virtual void StartLoading(Action onLoadComplete, Action loaderStep)
        {
            this.onLoadComplete = onLoadComplete;
            this.onLoaderStep = loaderStep;
        }

        public virtual void StartUnloading(Action onComplete)
        {
            onUnloadComplete = onComplete;
        }

        protected virtual void LoadComplete()
        {
            onLoadComplete.Invoke();
        }

        protected virtual void UnloadComplete()
        {
            onUnloadComplete.Invoke();
        }

        public int InitAndGetLoadersAmount()
        {
            CreateManagerOrder();
            return ManagerLoadingOrder.Count;
        }

        protected virtual void CreateManagerOrder()
        {
        }
    }
}