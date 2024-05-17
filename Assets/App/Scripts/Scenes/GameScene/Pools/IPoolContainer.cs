using App.Scripts.General.Infrastructure;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Pools
{
    public interface IPoolContainer : IRestartable
    {
        TItem GetItem<TItem>(PoolTypeId poolTypeId) where TItem : MonoBehaviour;
        void RemoveItem<TItem>(PoolTypeId poolTypeId, TItem item) where TItem : MonoBehaviour;
    }
}