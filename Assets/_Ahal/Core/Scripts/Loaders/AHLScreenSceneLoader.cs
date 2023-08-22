using UnityEngine.SceneManagement;

namespace AHL.Core.Loaders
{
    public class AHLScreenSceneLoader : AHLScreenLoader
    {
        public override void LoadScreen()
        {
            base.LoadScreen();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(screenTypeLoading.ToString());
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            FinishedLoad();
        }
    }
}