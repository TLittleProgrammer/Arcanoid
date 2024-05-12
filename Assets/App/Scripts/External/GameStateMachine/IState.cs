namespace App.Scripts.External.GameStateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IState<TParam> : IExitableState
    {
        void Enter(TParam param);
    }
}