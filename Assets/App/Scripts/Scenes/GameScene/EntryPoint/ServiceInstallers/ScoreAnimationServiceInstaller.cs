using App.Scripts.Scenes.GameScene.ScoreAnimation;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class ScoreAnimationServiceInstaller : Installer<ScoreAnimationServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IScoreAnimationService>().To<ScoreAnimationService>().AsSingle();
        }
    }
}