using DG.Tweening;

namespace App.Scripts.Scenes.GameScene.Dotween
{
    public interface ITweenersLocator
    {
        void AddTweener(Tweener tweener);
        void AddSequence(Sequence sequence);
        void RemoveTweener(Tweener tweener);
        void RemoveAll();
    }
}