using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants
{
    public interface IAngleCorrector
    {
        float CorrectAngleByDirection(Vector2 direction);
    }
}