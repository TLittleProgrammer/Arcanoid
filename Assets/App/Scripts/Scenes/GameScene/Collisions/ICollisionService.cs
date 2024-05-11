namespace App.Scripts.Scenes.GameScene.Collisions
{
    public interface ICollisionService<TEntity>
    {
        void Collide(TEntity entity);
    }
}