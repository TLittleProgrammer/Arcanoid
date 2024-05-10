using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Entities;

namespace App.Scripts.Scenes.GameScene.Containers
{
    public class EntityColliderContainer : IContainer<IBoxColliderable2D>
    {
        private List<IBoxColliderable2D> _entityViews;

        public EntityColliderContainer()
        {
            _entityViews = new();
        }
        
        public void AddItem(IBoxColliderable2D item)
        {
            _entityViews.Add(item);
        }

        public void RemoveItem(IBoxColliderable2D item)
        {
            _entityViews.Remove(item);
        }

        public IEnumerable<IBoxColliderable2D> GetItems()
        {
            return _entityViews;
        }
    }
}