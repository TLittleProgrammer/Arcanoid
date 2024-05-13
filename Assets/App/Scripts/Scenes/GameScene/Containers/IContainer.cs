using System;
using System.Collections.Generic;

namespace App.Scripts.Scenes.GameScene.Containers
{
    public interface IContainer<TItem> : IDisposable
    {
        void AddItem(TItem item);
        void RemoveItem(TItem item);
        IEnumerable<TItem> GetItems();
    }
}