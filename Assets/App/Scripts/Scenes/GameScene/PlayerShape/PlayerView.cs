using App.Scripts.Scenes.GameScene.Interfaces;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PlayerShape
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PlayerView : MonoBehaviour, ITransformable, ISpriteRenderable
    {
        private SpriteRenderer _spriteRenderer;
        
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public SpriteRenderer SpriteRenderer => _spriteRenderer ??= GetComponent<SpriteRenderer>();
    }
}