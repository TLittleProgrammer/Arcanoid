using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.Bootstrap.States;
using Zenject;

namespace App.Scripts.Scenes.Bootstrap.Installers
{
    public class LoadingSceneInstaller : MonoInstaller, IInitializable
    {
        [Inject] private IGameStateMachine _gameStateMachine;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LoadingSceneInstaller>().FromInstance(this).AsSingle();
        }

        public void Initialize()
        {
            _gameStateMachine.Enter<LoadingSceneState, string>("1.MainMenu");
        }
    }
}