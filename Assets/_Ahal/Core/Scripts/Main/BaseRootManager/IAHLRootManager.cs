using UnityEngine;

namespace AHL.Core.Main
{
    public interface IAHLRootManager
    {
        public void SetInjectedData();

        public AHLScreenInjectionDataScriptableObject GetScreenInjectedScriptable()
        {
            return Resources.Load<AHLScreenInjectionDataScriptableObject>("ScriptableObjects/AHLScreenInjectionDataScriptableObject");
        }
    }
}