using AHL.Core.Main;
using AHL.Core.Screen;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AHL.Core.Loaders
{
    public class AHLGameLoader : AHLMonoBehaviour
    {
        private const string LOG_TAG = "#GameLoader# ";

        [SerializeField] protected AHLBaseLoader managersLoader;

        [SerializeField] private Image fillerImage;
        [SerializeField] private float loaderTime = 1;

        private int currentLoader;
        private float loaderWeight;
        
        private void Start()
        {
            StartLoading();
        }

        private void OnDestroy()
        {
            DOTween.KillAll();
        }

        private void StartLoading()
        {
            fillerImage.fillAmount = 0;
            loaderWeight = 1f / (managersLoader.InitAndGetLoadersAmount());
            Application.targetFrameRate = 30;
            LoadManagers();
        }

        protected void LoadManagers()
        {
            Init(new AHLManager());
            managersLoader.Init(AHLManager);
            managersLoader.StartLoading(OnGameLoadComplete, OnLoaderStep);
        }

        private void OnLoaderStep()
        {
            currentLoader++;

            fillerImage.DOKill();
            fillerImage.DOFillAmount(currentLoader * loaderWeight, loaderTime);
        }

        private void OnGameLoadComplete()
        {
            AHLManager.Screens.ChangeScreen(ScreenTypes.GameScene);
            AHLManager.IsGameLoaded = true;
        }
    }
}