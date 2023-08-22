using System;
using System.Collections.Generic;
using AHL.Core.Events;
using AHL.Core.General;
using AHL.Core.General.Utils;
using AHL.Core.Pool;
using AHL.Core.Screen;
using UnityEngine;

namespace AHL.Core.Loaders
{
    public class AHLManagerLoader : AHLBaseLoader
    {
        private const string LOG_TAG = "#ManagerLoader#";

        private List<AHLBaseManager> ManagersOrderRelease = new();

        private Action onLoadStep;
        
        protected override void CreateManagerOrder()
        {
            ManagerLoadingOrder = new()
            {
                {typeof(AHLScreensManager), obj => { AHLManager.Screens = (IAHLScreensManager) obj; }},
                {typeof(AHLEventsManager), obj => { AHLManager.Events = (IAHLEventsManager) obj; }},
                {typeof(AHLPoolManager), obj => { AHLManager.Pool = (IAHLPoolManager) obj; }}
            };
        }

        public override void StartLoading(Action onLoadComplete, Action loaderStep)
        {
            AHLDebug.Log($"{LOG_TAG} Init");
            onLoadStep = loaderStep;

            base.StartLoading(onLoadComplete, onLoadStep);
            LoadNextManager();
        }

        private int currentManagerIndex = -1;

        private void LoadNextManager()
        {
            onLoadStep.Invoke();
            currentManagerIndex++;

            var managerType = ManagerLoadingOrder.Keys[currentManagerIndex];
            var managerCompleteAction = ManagerLoadingOrder.Values[currentManagerIndex];

            var isLastIndex = currentManagerIndex >= ManagerLoadingOrder.Count - 1;
            managerCompleteAction += isLastIndex ? _ => OnManagersLoadedComplete() : _ => LoadNextManager();
            Activator.CreateInstance(managerType, AHLManager, managerCompleteAction);
        }

        private void OnManagersLoadedComplete()
        {
            AHLDebug.Log($"{LOG_TAG} InitManagers Complete");

            LoadComplete();
        }

        public override void StartUnloading(Action onComplete)
        {
            base.StartUnloading(onComplete);
            ReleaseManagers(onComplete);
        }

        public void ReleaseManagers(Action onComplete)
        {
            ManagersOrderRelease = new()
            {
                (AHLBaseManager) AHLManager.Pool, (AHLBaseManager) AHLManager.Events
            };

            AHLDebug.Log($"{LOG_TAG} ReleaseManagers Start");
            AHLManager.IsGameCurrentlyUnloading = true;

            if (ManagersOrderRelease.Count <= 0)
            {
                AHLDebug.Log($"{LOG_TAG} ReleaseManagers Complete");
                AHLManager.IsGameCurrentlyUnloading = false;
                onComplete?.Invoke();
                return;
            }

            var managerToUnload = ManagersOrderRelease[0];
            ManagersOrderRelease.Remove(managerToUnload);

            managerToUnload.ReleaseManager(() =>
            {
                AHLDebug.Log($"{LOG_TAG} Unloading Manager - {managerToUnload.GetType().FullName}");

                //Each release better to be in a separate frame in order not to feel like a freeze
                WaitForFrame(() => ReleaseManagers(onComplete));
            });
        }
    }
}