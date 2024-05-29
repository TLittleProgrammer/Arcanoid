using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer
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