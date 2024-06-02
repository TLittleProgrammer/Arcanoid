using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Features.Helpers;
using App.Scripts.Scenes.GameScene.Features.ServiceActivator;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.Scenes.GameScene.States
{
    public class GameLoopState : IState, ITickable
    {
        private readonly List<ITickable> _tickables;
        private readonly IServicesActivator _servicesActivator;
        private readonly GameLoopSubscriber _gameLoopSubscriber;

        private bool _isActive;

        public GameLoopState(
            IServicesActivator servicesActivator,
            GameLoopSubscriber gameLoopSubscriber,
            List<ITickable> tickables)
        {
            _tickables = tickables;
            _servicesActivator = servicesActivator;
            _gameLoopSubscriber = gameLoopSubscriber;
        }

        public async UniTask Enter()
        {
            _servicesActivator.SetActiveToServices(true);
            _gameLoopSubscriber.SubscribeAll();
            _isActive = true;

            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            _servicesActivator.SetActiveToServices(false);
            _gameLoopSubscriber.DescribeAll();
            _isActive = false;

            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (_isActive is false)
                return;

            foreach (ITickable tickable in _tickables)
            {
                tickable.Tick();
            }
        }
    }
}