using System;
using System.Collections.Generic;
using System.Linq;
using AHL.Core.General;
using AHL.Core.General.Utils;
using AHL.Core.Main;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AHL.Core.Screen
{
    public sealed class AHLScreensManager : AHLBaseManager, IAHLScreensManager
    {
        private const string LOG_TAG = "#AHLScreensManager# ";
        private const string SCREEN_LOAD_BACK_BTN_LOCK = "screen_load_back_btn_lock";

        private Stack<ScreenTypes> screenTypeHistory = new();

        // //TODO: Set Types
        // private readonly Dictionary<ScreenTypes, string> screensLoaders = new()
        // {
        //     {ScreenTypes.TempBattleScene, "general_screen_loader"},
        //     {ScreenTypes.Menu, "general_screen_loader"}
        // };

        public bool IsChangingScreen { get; set; }

        private Dictionary<string, GameObject> currentScreenItems;
        private readonly AHLScreenInjectionDataScriptableObject screenInjectedScriptable;

        public AHLScreensManager(AHLManager manager, Action<AHLBaseManager> onComplete) : base(manager, onComplete)
        {
            screenTypeHistory.Push(ScreenTypes.None);
            screenInjectedScriptable = Resources.Load<AHLScreenInjectionDataScriptableObject>("ScriptableObjects/AHLScreenInjectionDataScriptableObject");
            OnInitComplete();
        }

        ~AHLScreensManager()
        {
        }

        public void ChangeScreen(ScreenTypes screenType)
        {
            SceneManager.LoadScene(screenType.ToString());
        }


        public ScreenTypes GetCurrentScreenType()
        {
            return screenTypeHistory.Peek();
        }

        public ScreenTypes GetPrevScreenType()
        {
            return screenTypeHistory.Count <= 1 ? ScreenTypes.None : screenTypeHistory.ElementAt(1);
        }

        public override void ReleaseManager(Action action)
        {
            screenTypeHistory = new();
            base.ReleaseManager(action);
        }

        public void SetScreenItems(Dictionary<string, GameObject> items)
        {
            currentScreenItems = items;
        }

        public void ClearScreenItems()
        {
            if (currentScreenItems.IsNullOrEmpty())
            {
                return;
            }

            foreach (var item in currentScreenItems)
            {
                Object.Destroy(item.Value);
            }

            currentScreenItems.Clear();
        }
        
        public void SetScreenInjectedData(IAHLScreenInjectableData screenInjectedData)
        {
            screenInjectedScriptable.ScreenInjectedData = screenInjectedData;
        }
    }
}