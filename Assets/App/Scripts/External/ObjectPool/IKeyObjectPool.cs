namespace App.Scripts.External.ObjectPool
{
    public interface IKeyObjectPool<TType>
    {
        TType Spawn(string key);
        void Despawn(string key, object item);
        void Clear();
    }
}