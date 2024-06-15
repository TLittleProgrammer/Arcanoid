using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Animator
{
    public interface IMonoAnimator
    {
        UniTask Animate();
    }
}