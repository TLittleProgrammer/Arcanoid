using App.Scripts.Scenes.GameScene.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BallView : MonoBehaviour, IPositionable, ISpriteRenderable
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