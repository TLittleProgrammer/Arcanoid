using System;
using TMPro;

namespace App.Scripts.Scenes.GameScene.Features.ScoreAnimation
{
    public interface IScoreAnimationService
    {
        void Animate(TMP_Text text, int from, int to);
        void Animate(TMP_Text text, int from, int to, Action<int> updateValue);
    }
}