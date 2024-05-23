using App.Scripts.External.GameStateMachine;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.States
{
    public class PopupState : IState
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