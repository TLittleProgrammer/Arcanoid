using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Time
{
    public interface ITimeScaleAnimator
    {
        UniTask Animate(float scaleTo);
    }
}