using App.Scripts.External.GameStateMachine;
using App.Scripts.General.States;
using Zenject;

namespace App.Scripts.Scenes.Bootstrap.Installers
{
    public class LoadingSceneInstaller : MonoInstaller, IInitializable
    {
        [Inject] private IStateMachine _stateMachine;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LoadingSceneInstaller>().FromInstance(this).AsSingle();
        }

        public void Initialize()
        {
            _stateMachine.Enter<LoadingSceneState, string, bool>("1.MainMenu", true);
        }
    }
    
}