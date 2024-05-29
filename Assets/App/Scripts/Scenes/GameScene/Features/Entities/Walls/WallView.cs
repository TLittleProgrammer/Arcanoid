using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Walls
{
    public class WallView : MonoBehaviour, IBoxColliderable2D
    {
        [SerializeField] private BoxCollider2D _boxCollider2D;
        public BoxCollider2D BoxCollider2D => _boxCollider2D;
    }
}