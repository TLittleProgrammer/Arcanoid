using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.Entities
{
    public class EntityView : MonoBehaviour, IEntity
    {
        [SerializeField]
        private EntityTypeId _entityTypeId;

        public EntityTypeId EntityTypeId
        {
            get => _entityTypeId;
            set => _entityTypeId = value;
        }
        
        public int2 GridPosition { get; set; }
    }
}