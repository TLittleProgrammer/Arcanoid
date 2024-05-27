using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.ServiceActivator
{
    public sealed class ServiceActivator : IServicesActivator
    {
        private List<IActivable> _activablesList = new();
        
        public void AddActivable(IActivable activable)
        {
            _activablesList.Add(activable);
        }

        public void RemoveActivable(IActivable activable)
        {
            _activablesList.Remove(activable);
        }

        public void SetActiveToServices(bool value)
        {
            foreach (IActivable activable in _activablesList)
            {
                activable.IsActive = value;
            }
        }
    }
}