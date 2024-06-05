namespace App.Scripts.External.ObjectPool
{
    public interface IKeyObjectPool<TType>
    {
        TType Spawn(string key);
        void Despawn(TType type);
    }
}