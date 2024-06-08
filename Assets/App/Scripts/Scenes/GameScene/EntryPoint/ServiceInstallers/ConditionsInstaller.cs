using App.Scripts.Scenes.GameScene.Features.Effects.Conditions;
using App.Scripts.Scenes.GameScene.Features.Effects.Conditions.Concrete;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class ConditionsInstaller : Installer<ConditionsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CircleCondition>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExplosionByFireballCondition>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ConditionService>().AsSingle();
        }
    }
}