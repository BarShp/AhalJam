using System;
using AHL.Core.Screen;
using UnityEngine;

namespace AHL.Core.Loaders
{
    public class AHLScreenPrefabsLoader : AHLScreenLoader
    {
        protected virtual string[] prefabsToLoad { get;}
        
        public override void StartLoadScreen(ScreenTypes screenType, Action onLoadFinished)
        {
            base.StartLoadScreen(screenType, onLoadFinished);
            
            WaitForTimeSeconds(loadUpTime, LoadScreenPrefabs);
        }

        protected virtual void LoadScreenPrefabs()
        {
            // AHLManager.Factory.GenerateAndCacheBatchAssets<GameObject>(prefabsToLoad, items =>
            // {
            //     AHLManager.Screens.SetScreenItems(items);
            //     FinishedLoad();
            // });
        }
    }
}