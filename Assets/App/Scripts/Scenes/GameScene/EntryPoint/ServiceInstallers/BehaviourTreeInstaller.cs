using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot;
using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class BehaviourTreeInstaller : Installer<BehaviourTreeInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AutopilotMoveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GetBottomUsableItemService>().AsSingle();
            Container.Bind<SimpleMovingStrategy>().AsSingle();
            Container.Bind<StickyStrategy>().AsSingle();
            
            Container.Bind<BehaviourTree>().AsSingle();
        }
    }
}