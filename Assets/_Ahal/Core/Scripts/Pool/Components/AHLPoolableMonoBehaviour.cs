// using AHL.Core.Main;

namespace AHL.Core.Pool.Components
{
    public abstract class AHLPoolableMonoBehaviour //: AHLMonoBehaviour, IAHLPoolable
    {
        public virtual string PoolID { get; set; }
        public string PooledObjectID { get; set; }
        public bool IsInPool { get; set; }

        public abstract void OnReturnedToPool();
    }
}