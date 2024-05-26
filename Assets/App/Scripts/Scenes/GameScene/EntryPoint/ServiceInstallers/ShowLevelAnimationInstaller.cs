using App.Scripts.Scenes.GameScene.Features.Levels.Animations;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class ShowLevelAnimationInstaller : Installer<ShowLevelAnimationInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IShowLevelAnimation>().To<SimpleShowLevelAnimation>().AsSingle();
        }
    }
}