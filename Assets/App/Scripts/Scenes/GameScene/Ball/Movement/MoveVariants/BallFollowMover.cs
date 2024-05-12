using App.Scripts.Scenes.GameScene.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public sealed class BallFollowMover : IBallFollowMover
    {
        private readonly IPositionable _ballPositionable;
        private readonly IPositionable _targetPositionable;

        public BallFollowMover(IPositionable ballPositionable, IPositionable targetPositionable)
        {
            _ballPositionable = ballPositionable;
            _targetPositionable = targetPositionable;
        }

        public async UniTask AsyncInitialize()
        {
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
    }
}