using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Pools
{
    public interface IPoolContainer
    {
        TItem GetItem<TItem>(PoolTypeId poolTypeId) where TItem : MonoBehaviour;
        void RemoveItem<TItem>(PoolTypeId poolTypeId, TItem item) where TItem : MonoBehaviour;
    }
}