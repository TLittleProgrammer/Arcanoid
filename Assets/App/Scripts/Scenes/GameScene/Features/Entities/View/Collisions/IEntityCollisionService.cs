namespace App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions
{
    public interface IEntityCollisionService
    {
        void AddEntity(IEntityView entityView);
        void Clear();
    }
}