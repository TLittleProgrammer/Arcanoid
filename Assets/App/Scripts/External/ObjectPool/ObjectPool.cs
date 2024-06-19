using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.External.ObjectPool
{
    public abstract class ObjectPool<TType> : IObjectPool, IKeyableObjectPool<TType>
    {
        private readonly Func<TType> _spawner;
        private readonly Queue<TType> _items = new();

        private const int DefaultPoolSize = 10;

        protected ObjectPool()
        {
            
        }

        protected ObjectPool(Func<TType> spawner)
        {
            _spawner = spawner;
        }

        public string Key { get; set; }

        protected ObjectPool(Func<TType> spawner, int initialSize)
        {
            _spawner = spawner;
            Resize(initialSize);
        }

        public int ItemCount => _items.Count;


        public void Resize(int itemCount)
        {
            int itemCountDifference = itemCount - ItemCount;

            AddElements(itemCountDifference);
            RemoveElements(-itemCountDifference);
        }

        public TType Spawn()
        {
            TType item = ItemCount > 0 ? _items.Dequeue() : CreateItem();
            OnItemSpawned(item);

            return item;
        }

        public void Despawn(object item)
        {
            if (item is TType targetItem)
            {
                _items.Enqueue(targetItem);
                OnDespawnItem(targetItem);
            }
        }

        public void Clear()
        {
            _items.Clear();
        }

        protected virtual void OnDespawnItem(TType targetItem)
        {
            
        }

        protected virtual void OnRemoveItem(TType item)
        {
            
        }

        protected virtual void OnItemSpawned(TType item)
        {
            
        }

        protected virtual void OnItemCreated(TType item)
        {
            
        }

        private TType CreateItem()
        {
            TType item = _spawner.Invoke();
            OnItemCreated(item);

            return item;
        }

        private void AddElements(int addCount)
        {
            for (int i = 0; i < addCount; i++)
            {
                TType item = CreateItem();
                _items.Enqueue(item);
            }
        }

        private void RemoveElements(int removeCount)
        {
            for (int i = 0; i < removeCount; i++)
            {
                TType item = _items.Dequeue();
                OnRemoveItem(item);
            }
        }
    }
}