using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace AHL.Core.Pool
{
    public interface IAHLPoolManager
    {
        int ActiveObjectCount(string poolName);
        int ArchivedObjectCount(string poolName);
        int TotalObjectCount(string poolName);
        void InitPools(Dictionary<string, int> pools, UnityAction onInit = null, string tag = "");
        void InitPool(string addressableName, int amount, UnityAction onInit = null, string tag = "", string overrideName = "");
        public void AddMorePoolablesToPool(string poolName, int amount);
        bool IsPoolExist(string poolName);
        T GetPoolable<T>(string poolName) where T : IAHLPoolable;
        void ReturnPoolable(IAHLPoolable poolObj);
        int GetTotalPoolCount(string poolName);
        void ClearPool(string poolName);
        void ClearAllPools();
        void ClearAllPoolsByTag(string tag);
        void ReleaseManager(Action action);
    }
}