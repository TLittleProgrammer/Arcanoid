using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Time
{
    public interface ITimeScaleAnimator
    {
        UniTask Animate(float scaleTo);
    }
}