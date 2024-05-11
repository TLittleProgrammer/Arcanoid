using App.Scripts.External.Grid;
using App.Scripts.Scenes.GameScene.Entities;

namespace App.Scripts.Scenes.GameScene.Levels.View
{
    public interface ILevelViewUpdater
    {
        void SetGrid(Grid<int> grid);
        void UpdateVisual(IEntityView entityView);
    }
}