using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.ServiceActivator
{
    public interface IServicesActivator
    {
        void AddActivable(IActivable activable);
        void RemoveActivable(IActivable activable);
        void SetActiveToServices(bool value);
    }
}