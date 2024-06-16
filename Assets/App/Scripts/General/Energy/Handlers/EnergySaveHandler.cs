using App.Scripts.External.UserData;
using App.Scripts.General.Game;
using App.Scripts.General.UserData.Global;
using Zenject;

namespace App.Scripts.General.Energy.Handlers
{
    public sealed class EnergySaveHandler : IEnergySaveHandler, IInitializable
    {
        private readonly IGameStatements _gameStatements;
        private readonly IEnergyInitializer _energyInitializer;

        public EnergySaveHandler(IGameStatements gameStatements, IEnergyInitializer energyInitializer)
        {
            _gameStatements = gameStatements;
            _energyInitializer = energyInitializer;
        }

        public void Initialize()
        {
            _gameStatements.ApplicationFocus += OnApplicationFocusChanged;
        }

        public void Save()
        {
        }

        public void Load()
        {
            _energyInitializer.Initialize();
        }

        private void OnApplicationFocusChanged(bool focus)
        {
            if (focus)
            {
                Load();
            }
            else
            {
                Save();
            }
        }
    }
}