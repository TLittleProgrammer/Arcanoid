using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Zenject;

namespace App.Scripts.Tools.Editor.LevelEditor
{
    public class CheatPanel : OdinEditorWindow
    {
        private SceneContext _sceneContext;
        private ProjectContext _projectContext;

        public int EnergyCounter;
        public int HealthCounter;
        
        private static void OpenWindow()
        {
            GetWindow<CheatPanel>().Show();
        }

        [OnInspectorInit]
        private void CreateGrid()
        {
            
        }

        [Button]
        private void AddEnergy()
        {
            InitProjectContext();

            var energyDataService = _projectContext.Container.Resolve<IEnergyDataService>();
            
            energyDataService.Add(EnergyCounter);
        }
        
        [Button]
        private void ChangeHealth()
        {
            InitSceneContext();

            var energyDataService = _sceneContext.Container.Resolve<IHealthContainer>();
            
            energyDataService.UpdateHealth(HealthCounter, false);
        }

        [Button]
        private void SkipLevel()
        {
            InitSceneContext();

            var skipLevelCommand = _sceneContext.Container.Resolve<ISkipLevelCommand>();
            
            skipLevelCommand.Execute();
        }

        private void InitSceneContext()
        {
            _sceneContext = FindObjectOfType<SceneContext>();   
        }
        
        private void InitProjectContext()
        {
            _projectContext = FindObjectOfType<ProjectContext>();
        }
    }
}