using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Scripts.External.GameStateMachine
{
    public sealed class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;

        private IExitableState _activeState;
        
        public GameStateMachine(IEnumerable<IExitableState> states)
        {
            _states = states
                .ToDictionary(x => x.GetType(), x => x);
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            
            state.Enter();
        }

        public void Enter<TState, TParam>(TParam param) where TState : class, IState<TParam>
        {
            TState state = ChangeState<TState>();
            
            state.Enter(param);
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