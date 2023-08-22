using System.Collections.Generic;
using UnityEngine;

namespace AHL.Core.Pool
{
    public class AHLPoolData
    {
        public AHLPoolData()
        {
            totalObjectPool = new Queue<IAHLPoolable>();
            archiveObjectPool = new Queue<IAHLPoolable>();
        }

        public string poolTag;
        public Transform parent;
        public readonly Queue<IAHLPoolable> totalObjectPool;
        public readonly Queue<IAHLPoolable> archiveObjectPool;

        public int TotalCount => totalObjectPool.Count;
    }
}