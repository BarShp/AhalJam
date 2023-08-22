using UnityEngine;

namespace AHL.Core.Main
{
    [CreateAssetMenu(menuName = "AHL/InjectionDataScriptableObject", fileName = "AHLScreenInjectionDataScriptableObject", order = 0)]
    public class AHLScreenInjectionDataScriptableObject : ScriptableObject
    {
        [SerializeField] public IAHLRootManager AHLManager;
        public object ScreenInjectedData { private get; set; }

        public T GetInjectedData<T>() where T : IAHLScreenInjectableData
        {
            return (T) ScreenInjectedData;
        }
    }

    public interface IAHLScreenInjectableData
    {
    }
}