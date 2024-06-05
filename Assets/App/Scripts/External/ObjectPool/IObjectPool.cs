namespace App.Scripts.External.ObjectPool
{
    public interface IObjectPool
    {
        int ItemCount { get; }

        void Resize(int itemCount);
        void Despawn(object item);
        void Clear();
    }

    public interface IObjectPool<out TType>
    {
        TType Spawn();
    }
}