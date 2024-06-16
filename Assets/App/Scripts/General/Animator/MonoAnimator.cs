using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.General.Animator
{
    public abstract class MonoAnimator : MonoBehaviour, IMonoAnimator
    {
        public abstract UniTask Animate();
        public abstract UniTask UndoAnimate();
    }
}