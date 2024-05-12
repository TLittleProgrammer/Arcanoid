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
    public interface IState<TParam, TSecondParam> : IExitableState
    {
        void Enter(TParam param, TSecondParam secondParam);
    }
}