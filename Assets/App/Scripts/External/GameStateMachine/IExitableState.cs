using Cysharp.Threading.Tasks;

namespace App.Scripts.External.GameStateMachine
{
    public interface IExitableState
    {
        UniTask Exit();
    }
}