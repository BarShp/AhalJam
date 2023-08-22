namespace AHL.Core.Pool
{
    public interface IAHLPoolable
    {
        string PoolID { get; set; }
        string PooledObjectID { get; set; }
        public bool IsInPool { get; set; }

        void OnReturnedToPool();
    }
}