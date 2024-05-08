using Unity.Mathematics;

namespace GameScene.Levels.Entities
{
    public interface IEntity
    {
        EntityTypeId EntityTypeId { get; set; }
        int2 GridPosition { get; set; }
    }
}