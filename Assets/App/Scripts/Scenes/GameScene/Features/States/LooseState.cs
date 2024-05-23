using App.Scripts.External.GameStateMachine;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.States
{
    public class LooseState : IState
    {
        public async UniTask Enter()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}