using App.Scripts.Scenes.GameScene.Features.Autopilot;
using App.Scripts.Scenes.GameScene.Features.Autopilot.Strategies;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class BehaviourTreeInstaller : Installer<BehaviourTreeInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<SimpleMovingStrategy>().AsSingle();
            
            Container.Bind<BehaviourTree>().AsSingle();
        }
    }
}