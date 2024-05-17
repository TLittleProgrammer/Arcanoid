using App.Scripts.Scenes.GameScene.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Walls
{
    public class WallView : MonoBehaviour, IBoxColliderable2D
    {
        [SerializeField] private BoxCollider2D _boxCollider2D;
        public BoxCollider2D BoxCollider2D => _boxCollider2D;
    }
}