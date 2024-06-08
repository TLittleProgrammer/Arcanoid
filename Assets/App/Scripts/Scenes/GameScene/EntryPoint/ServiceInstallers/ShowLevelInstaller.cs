using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class ShowLevelInstaller : Installer<ShowLevelInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SimpleShowLevelAnimation>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnimationInDiagonals>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ShowLevelService>().AsSingle();
        }
    }
}