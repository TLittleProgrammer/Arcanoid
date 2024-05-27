using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LooseState : IState
    {
        private readonly ITimeProvider _timeProvider;

        public LooseState(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }
        
        public async UniTask Enter()
        {
            _timeProvider.TimeScale = 0f;

            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}