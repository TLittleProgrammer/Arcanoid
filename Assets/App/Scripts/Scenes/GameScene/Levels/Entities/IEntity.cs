using Unity.Mathematics;

namespace App.Scripts.Scenes.GameScene.Levels.Entities
{
    public interface IEntity
    {
        EntityTypeId EntityTypeId { get; set; }
        int2 GridPosition { get; set; }
    }
}