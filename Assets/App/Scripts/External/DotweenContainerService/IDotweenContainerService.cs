using DG.Tweening;

namespace App.Scripts.External.DotweenContainerService
{
    public interface IDotweenContainerService
    {
        void AddTween(Tween tween);
        void RemoveTween(Tween tween);
    }
}