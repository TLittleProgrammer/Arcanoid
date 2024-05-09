using App.Scripts.Scenes.GameScene.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public sealed class BallFollowMover : IBallMover
    {
        private readonly ITransformable _ballTransformable;
        private readonly ITransformable _targetTransformable;

        public BallFollowMover(ITransformable ballTransformable, ITransformable targetTransformable)
        {
            _ballTransformable = ballTransformable;
            _targetTransformable = targetTransformable;
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