using System;
using System.Collections.Generic;
using System.Linq;
using AHL.Core.General;
using AHL.Core.Main;
using AHL.Core.Screen;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace AHL.Core.Pool
{
    public sealed class AHLPoolManager : AHLBaseManager, IAHLPoolManager
    {
        private const string LOG_TAG = "#AHLPoolManager# ";
        private const string POOL_HOLDER = "PoolsHolder";

        private Dictionary<string, AHLPoolData> pools = new();
        private Transform poolsHolder;

        public AHLPoolManager(AHLManager manager, Action<AHLBaseManager> onComplete) : base(manager, onComplete)
        {
            InitPoolManager();
            // manager.Events.AddEventListener<AHLScreenChangeStartEvent>(OnScreenChangeStart);
            OnInitComplete();
        }
        
        ~AHLPoolManager()
        {
            // AHLManager.Events.RemoveEventListener<AHLScreenChangeStartEvent>(OnScreenChangeStart);
        
            ClearAllPools();
        }

        private void InitPoolManager()
        {
            poolsHolder = new GameObject(POOL_HOLDER).transform;
            Object.DontDestroyOnLoad(poolsHolder.gameObject);
        }

        // private void GenerateObjects(string addressableName, int amount, UnityAction onGenerated = null, string overrideName = "")
        // {
        //     var pool = pools[addressableName];
        //
        //     var request = new AHLAssetsRequest<Transform>
        //     {
        //         AssetName = addressableName,
        //         Amount = amount,
        //         OnComplete = delegate(Transform[] createdList)
        //         {
        //             foreach (var createdObj in createdList)
        //             {
        //                 var createdObjPoolable = createdObj.GetComponent<IAHLPoolable>();
        //                 createdObjPoolable.PoolID = addressableName;
        //                 createdObjPoolable.PooledObjectID = pools[addressableName].TotalCount.ToString();
        //                 createdObjPoolable.IsInPool = true;
        //                 
        //                 pools[addressableName].totalObjectPool.Enqueue(createdObjPoolable);
        //                 pools[addressableName].archiveObjectPool.Enqueue(createdObjPoolable);
        //                 
        //                 createdObj.transform.SetParent(pool.parent);
        //                 createdObj.gameObject.SetActive(false);
        //
        //                 if (overrideName.IsNotNullOrEmpty())
        //                 {
        //                     createdObj.gameObject.name = overrideName;
        //                 }
        //             }
        //
        //             onGenerated?.Invoke();
        //         }
        //     };
        //
        //
        //     AHLManager.Factory.GenerateAssets(request);
        // }

        public int ActiveObjectCount(string addressableName)
        {
            if (!pools.ContainsKey(addressableName))
            {
                return 0;
            }

            return pools[addressableName].totalObjectPool.Count - pools[addressableName].archiveObjectPool.Count;
        }

        public int ArchivedObjectCount(string addressableName)
        {
            return !pools.ContainsKey(addressableName) ? 0 : pools[addressableName].archiveObjectPool.Count;
        }

        public int TotalObjectCount(string addressableName)
        {
            return !pools.ContainsKey(addressableName) ? 0 : pools[addressableName].totalObjectPool.Count;
        }

        public void InitPools(Dictionary<string, int> pools, UnityAction onInit = null, string tag = "")
        {
            var poolsMade = 0;

            foreach (var pool in pools)
            {
                InitPool(pool.Key, pool.Value, () =>
                {
                    poolsMade++;
                    if (poolsMade >= pools.Count)
                    {
                        onInit?.Invoke();
                    }
                }, tag);
            }
        }

        public void InitPool(string addressableName, int amount, UnityAction onInit = null, string tag = "", string overrideName = "")
        {
            if (addressableName == null)
            {
                // AHLManager.Monitor.LogHandledException("InitPool with value null");
                return;
            }
            
            if (IsPoolExist(addressableName))
            {
                onInit?.Invoke();
                return;
            }

            pools[addressableName] = new AHLPoolData {parent = new GameObject(addressableName).transform, poolTag = tag};
            pools[addressableName].parent.SetParent(poolsHolder);

            // AHLDebug.Log($"{LOG_TAG} InitPool - {addressableName}, amount: {amount}");

            // GenerateObjects(addressableName, amount, onInit, overrideName);
        }

        public void AddMorePoolablesToPool(string addressableName, int amount)
        {
            // GenerateObjects(addressableName, amount);
        }

        public bool IsPoolExist(string addressableName)
        {
            return pools.ContainsKey(addressableName);
        }

        public T GetPoolable<T>(string addressableName) where T : IAHLPoolable
        {
            if (!IsPoolExist(addressableName))
            {
                return default;
            }

            if (ArchivedObjectCount(addressableName) <= 0)
            {
                // AhalDebug.LogError($"GetPoolable was called with {addressableName} but not enough poolables left, Total = {GetTotalPoolCount(addressableName)}");
                return default;
            }
            
            var poolable = pools[addressableName].archiveObjectPool.Dequeue();
            poolable.IsInPool = false;
            return (T) poolable;
        }

        public void ReturnPoolable(IAHLPoolable poolObj)
        {
            poolObj.OnReturnedToPool();
            poolObj.IsInPool = true;
            
            var addressableName = poolObj.PoolID;

            if (!pools.ContainsKey(addressableName) || pools[addressableName] == null)
            {
                Object.Destroy((Object) poolObj);
                return;
            }
            
            ((MonoBehaviour) poolObj).transform.SetParent(pools[addressableName].parent);
            
            var pollMono = (MonoBehaviour) poolObj;
            pollMono.gameObject.SetActive(false);

#if ENABLE_LOGS
            if (pools[addressableName].archiveObjectPool.Contains(poolObj))
            {
                var poolableMonoBehaviour = (AHLMonoBehaviour) poolObj;
                AHLDebug.LogError($"Pool {addressableName} already contains a the returned poolable {poolableMonoBehaviour.name}.", poolableMonoBehaviour);
                // Do not add "return" here.
                //  If we arrived here, it means there's an error in the USAGE of the poolable, not in the poolable itself
                //  (someone tries to return the same item multiple times)
                //  Fix the wrong poolable usage rather than making it work here (also won't work on build)
            }
#endif
            
            pools[addressableName].archiveObjectPool.Enqueue(poolObj);
        }

        public int GetTotalPoolCount(string addressableName)
        {
            return pools[addressableName].totalObjectPool.Count;
        }
        

        public void ClearPool(string addressableName)
        {
            foreach (var poolable in pools[addressableName].totalObjectPool)
            {
                Object.Destroy((Object) poolable);
            }
            
            Object.Destroy(pools[addressableName].parent.gameObject);
            pools.Remove(addressableName);
        }

        public void ClearAllPools()
        {
            var addressableNames = pools.Keys.ToArray();

            foreach (var pool in addressableNames)
            {
                ClearPool(pool);
            }

            pools.Clear();
        }
        
        public void ClearAllPoolsByTag(string tag)
        {
            var addressableNames = pools.Where(x => x.Value.poolTag == tag).Select(x => x.Key).ToArray();

            foreach (var pool in addressableNames)
            {
                ClearPool(pool);
            }
        }
    }
}