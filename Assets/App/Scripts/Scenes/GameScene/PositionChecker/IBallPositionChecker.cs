namespace App.Scripts.Scenes.GameScene.PositionChecker
{
    public interface IBallPositionChecker : IPositionChecker
    {
        public CollisionTypeId CurrentCollisionTypeId { get; }
    }
}