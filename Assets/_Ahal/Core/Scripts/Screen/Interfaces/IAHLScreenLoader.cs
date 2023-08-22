using System;

namespace AHL.Core.Screen
{
    public interface IAHLScreenLoader
    {
        void StartLoadScreen(ScreenTypes screenType, Action onLoadFinished);
        void LoadScreen();
        void FinishedLoad();
        void OnEndLoadAnimation();
    }
}