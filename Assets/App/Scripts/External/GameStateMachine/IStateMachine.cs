using System.Collections.Generic;
using App.Scripts.External.Initialization;
using Cysharp.Threading.Tasks;

namespace App.Scripts.External.GameStateMachine
{
    public interface IStateMachine : IAsyncInitializable<List<IExitableState>>
    {
        IState CurrentState { get; }
        
        UniTask Enter<TState>() where TState : class, IState;
        UniTask Enter<TState, TParam>(TParam param) where TState : class, IState<TParam>;
    }
}