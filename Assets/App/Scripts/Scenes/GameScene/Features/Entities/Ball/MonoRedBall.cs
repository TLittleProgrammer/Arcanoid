using System;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball
{
    public sealed class MonoRedBall : MonoBehaviour
    {
        [SerializeField] private BallView _ballView;
        
        public Action<BallView, Collider2D> Collided;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            Collided?.Invoke(_ballView, col);
        }
    }
}