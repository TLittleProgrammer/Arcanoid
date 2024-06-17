using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Animator
{
    public interface IMonoAnimator
    {
        UniTask Animate();
    }

    public interface IMonoAnimator<TParam>
    {
        UniTask Animate(TParam param);
    }
    
    public interface IMonoAnimator<TParam1, TParam2>
    {
        UniTask Animate(TParam1 param1, TParam2 param2);
    }
}