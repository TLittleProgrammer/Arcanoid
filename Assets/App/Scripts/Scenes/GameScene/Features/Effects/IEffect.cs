using System.Diagnostics;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public interface IEffect
    {
        void PlayEffect();
        void StopEffect();
    }
}