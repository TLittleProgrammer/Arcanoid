using App.Scripts.Scenes.GameScene.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public sealed class BallFollowMover : IBallFollowMover
    {
        private readonly IPositionable _ballPositionable;
        private readonly IPositionable _targetPositionable;
        private Vector3 _offset;

        public BallFollowMover(IPositionable ballPositionable, IPositionable targetPositionable)
        {
            _ballPositionable = ballPositionable;
            _targetPositionable = targetPositionable;
            _offset = ballPositionable.Position - targetPositionable.Position;
        }

        public async UniTask AsyncInitialize()
        {
            _ballPositionable.Position =
                new Vector3
                (
                    _targetPositionable.Position.x + _offset.x,
                    _targetPositionable.Position.y + _offset.y,
                    0f
                );
            
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            _ballPositionable.Position =
                new Vector3
                (
                    _targetPositionable.Position.x,
                    _ballPositionable.Position.y,
                    0f
                );
        }

        public void UpdateSpeed(float addValue)
        {
            
        }

        public void Restart()
        {
            
        }
    }
}