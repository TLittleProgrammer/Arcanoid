using System;
using UnityEngine;

namespace App.Scripts.External.ObjectPool
{
    public class MonoObjectPool<TMono> : ObjectPool<TMono> where TMono : MonoBehaviour
    {
        private Transform _parent;

        public MonoObjectPool(Func<TMono> spawner, int initialSize, string parentName, string key) : base(spawner)
        {
            Key = key;

            Initialize(parentName);
            Resize(initialSize);
        }

        private void Initialize(string parentName)
        {
            GameObject parentGameObject = GameObject.Find(parentName);

            if (parentGameObject is not null)
            {
                _parent = parentGameObject.transform;
                return;
            }
            
            GameObject gameObject = new GameObject(parentName);
            _parent = gameObject.transform;
        }

        protected override void OnItemSpawned(TMono item)
        {
            base.OnItemSpawned(item);
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawnItem(TMono targetItem)
        {
            base.OnDespawnItem(targetItem);
            DeactiavateItem(targetItem);
        }

        protected override void OnItemCreated(TMono item)
        {
            base.OnItemCreated(item);
            SetParent(item);
            DeactiavateItem(item);
        }

        private void SetParent(TMono item)
        {
            item.transform.SetParent(_parent);
        }

        private void DeactiavateItem(TMono targetItem)
        {
            targetItem.gameObject.SetActive(false);
        }
    }
}