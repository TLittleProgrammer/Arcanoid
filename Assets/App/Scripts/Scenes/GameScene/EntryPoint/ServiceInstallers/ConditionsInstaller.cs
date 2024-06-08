using App.Scripts.Scenes.GameScene.Features.Effects.Conditions;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class ConditionsInstaller : Installer<ConditionsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CircleCondition>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExplosionCondition>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ConditionService>().AsSingle();
        }
    }
}