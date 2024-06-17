using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.General.Animator
{
    public abstract class UndoMonoAnimator : MonoBehaviour
    {
        public abstract UniTask UndoAnimate();
    }

    public abstract class MonoAnimator : UndoMonoAnimator, IMonoAnimator
    {
        public abstract UniTask Animate();
    }

    public abstract class MonoAnimator<TParam> : UndoMonoAnimator, IMonoAnimator<TParam>
    {
        public abstract UniTask Animate(TParam param);
    }
    
    public abstract class MonoAnimator<TParam1, TParam2> : UndoMonoAnimator, IMonoAnimator<TParam1, TParam2>
    {
        public abstract UniTask Animate(TParam1 param1, TParam2 param2);
    }
}