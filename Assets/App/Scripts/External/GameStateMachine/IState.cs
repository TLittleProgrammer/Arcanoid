using Cysharp.Threading.Tasks;

namespace App.Scripts.External.GameStateMachine
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }

    public interface IState<TParam> : IExitableState
    {
        UniTask Enter(TParam param);
    }
    
    public interface IState<TParam, TSecondParam> : IExitableState
    {
        UniTask Enter(TParam param, TSecondParam secondParam);
    }
}