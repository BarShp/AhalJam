using System;
using AHL.Core.Events;
using AHL.Core.Pool;
using AHL.Core.Screen;
using UnityEngine;

namespace AHL.Core.Main
{
    public class AHLManager : IAHLRootManager
    {
        private const string LOG_TAG = "AHLManager - ";
        
        //AHL Managers
        public IAHLScreensManager Screens;
        public IAHLEventsManager Events;
        public IAHLPoolManager Pool;
        
        private readonly Action managerInitComplete;
        private readonly Action loaderStep;

        public bool IsGameLoaded;
        public bool IsGameCurrentlyUnloading;

        public AHLManager()
        {
            SetInjectedData();    
        }

        public void SetUnloadStart()
        {
            IsGameCurrentlyUnloading = true;
        }

        public void SetUnloadFinished()
        {
            IsGameCurrentlyUnloading = false;
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }

        public void SetInjectedData()
        {
            (this as IAHLRootManager).GetScreenInjectedScriptable().AHLManager = this;
        }
    }
}