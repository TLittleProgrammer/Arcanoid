using DG.Tweening;

namespace App.Scripts.General.DotweenContainerService
{
    public interface IDotweenContainerService
    {
        void AddTween(Tween tween);
        void RemoveTween(Tween tween);
    }
}