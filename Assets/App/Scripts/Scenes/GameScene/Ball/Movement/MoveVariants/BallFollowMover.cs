using System.Threading.Tasks;
using App.Scripts.Scenes.GameScene.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public sealed class BallFollowMover : IBallFollowMover
    {
        private readonly ITransformable _ballTransformable;
        private readonly ITransformable _targetTransformable;

        public BallFollowMover(ITransformable ballTransformable, ITransformable targetTransformable)
        {
            _ballTransformable = ballTransformable;
            _targetTransformable = targetTransformable;
        }

        public async UniTask AsyncInitialize()
        {
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            _ballTransformable.Position =
                new Vector3
                (
                    _targetTransformable.Position.x,
                    _ballTransformable.Position.y,
                    0f
                );
        }
    }
}