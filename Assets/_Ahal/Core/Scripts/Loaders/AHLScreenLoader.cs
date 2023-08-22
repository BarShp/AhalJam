using System;
using AHL.Core.Main;
using AHL.Core.Screen;
using UnityEngine;

namespace AHL.Core.Loaders
{
    public class AHLScreenLoader : AHLMonoBehaviour, IAHLScreenLoader
    {
        [SerializeField] private Animator anim;
        [SerializeField] protected float loadUpTime = 1f;
        [SerializeField] protected float loadEndTime = 1f;

        protected ScreenTypes screenTypeLoading;
        private static readonly int StarLoad = Animator.StringToHash("StartLoad");
        private static readonly int EndLoad = Animator.StringToHash("EndLoad");

        private Action onLoadFinishedAction;

        public virtual void StartLoadScreen(ScreenTypes screenType, Action onLoadFinished)
        {
            screenTypeLoading = screenType;
            onLoadFinishedAction = onLoadFinished;

            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(true);

            if (anim != null)
            {
                anim.SetTrigger(StarLoad);
            }
            
            WaitForTimeSeconds(loadUpTime, LoadScreen);
        }

        public virtual void LoadScreen()
        {
            // AHLManager.Screens.ClearScreenItems();
        }

        public virtual void FinishedLoad()
        {
            WaitForTimeSeconds(0.5f, () =>
            {
                anim.SetTrigger(EndLoad);

                OnEndLoadAnimation();

                WaitForTimeSeconds(loadEndTime, delegate
                {
                    Destroy(gameObject);
                    onLoadFinishedAction.Invoke();
                });
            });
        }

        public virtual void OnEndLoadAnimation()
        {
        }
    }
}