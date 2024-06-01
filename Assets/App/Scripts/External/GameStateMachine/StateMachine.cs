using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace App.Scripts.External.GameStateMachine
{
    public sealed class StateMachine : IStateMachine
    {
        private Dictionary<Type, IExitableState> _states;

        private IExitableState _activeState;

        public UniTask AsyncInitialize(IEnumerable<IExitableState> param)
        {
            _states = param
                .ToDictionary(x => x.GetType(), x => x);
            return UniTask.CompletedTask;
        }

        public IState CurrentState => _activeState as IState;

        public UniTask Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            
            return  state.Enter();
        }

        public UniTask Enter<TState, TParam>(TParam param) where TState : class, IState<TParam>
        {
            TState state = ChangeState<TState>();
            
            return state.Enter(param);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}