using System.Collections.Generic;
using App.Scripts.External.Initialization;

namespace App.Scripts.External.GameStateMachine
{
    public interface IStateMachine : IAsyncInitializable<IEnumerable<IExitableState>>
    {
        IState CurrentState { get; }
        
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TParam>(TParam param) where TState : class, IState<TParam>;
        void Enter<TState, TParam, TSecondParam>(TParam param, TSecondParam secondParam) where TState : class, IState<TParam, TSecondParam>;
        void Enter<TState, TParam, TSecondParam, TThirdParam>(TParam param, TSecondParam secondParam, TThirdParam thirdParam) where TState : class, IState<TParam, TSecondParam, TThirdParam>;
    }
}