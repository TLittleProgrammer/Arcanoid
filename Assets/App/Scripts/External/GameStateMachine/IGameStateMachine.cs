namespace App.Scripts.External.GameStateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TParam>(TParam param) where TState : class, IState<TParam>;
    }
}