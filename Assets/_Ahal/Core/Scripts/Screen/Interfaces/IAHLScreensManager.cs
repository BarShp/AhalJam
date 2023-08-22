using System;
using System.Collections.Generic;
using AHL.Core.Main;
using UnityEngine;

namespace AHL.Core.Screen
{
    public interface IAHLScreensManager
    {
        void ChangeScreen(ScreenTypes screenType);
        ScreenTypes GetCurrentScreenType();
        void ReleaseManager(Action action);
    }
}