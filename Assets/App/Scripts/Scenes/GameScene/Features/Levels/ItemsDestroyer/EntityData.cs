using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer
{
    public sealed class EntityData
    {
        public GridItemData GridItemData;
        public IEntityView EntityView;

        public EntityData()
        {
            
        }
        
        public EntityData(GridItemData gridItemData, IEntityView entityView)
        {
            GridItemData = gridItemData;
            EntityView = entityView;
        }
    }
}