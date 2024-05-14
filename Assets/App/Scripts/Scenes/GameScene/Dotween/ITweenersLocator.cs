using DG.Tweening;

namespace App.Scripts.Scenes.GameScene.Dotween
{
    public interface ITweenersLocator
    {
        void AddTweener(Tweener tweener);
        void RemoveTweener(Tweener tweener);
        void RemoveAllTweeners();
    }
}