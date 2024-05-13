using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.PlayerShape;
using App.Scripts.Scenes.GameScene.PlayerShape.Move;

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

        public void Dispose()
        {
            var playerView = _entityViews.First(x => x is PlayerView);
            _entityViews.Clear();
            _entityViews.Add(playerView);
        }
    }
}