using App.Scripts.Scenes.GameScene.Features.Boosts.General.Systems;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public class FireballBoostActivator : IConcreteBoostActivator
    {
        private ILevelLoader _levelLoader;
        private IBallsService _ballsService;
        private readonly IFireballSystem _fireballSystem;

        public FireballBoostActivator(
        ILevelLoader levelLoader,
        IBallsService ballsService,
        IFireballSystem fireballSystem)
        {
            _levelLoader = levelLoader;
            _ballsService = ballsService;
            _fireballSystem = fireballSystem;
        }

        public bool IsTimeableBoost => true;

        public void Activate()
        {
            SetFlag(true);
        }

        public void Deactivate()
        {
            SetFlag(false);
        }

        private void SetFlag(bool flag)
        {
            _fireballSystem.IsActive = flag;
            _ballsService.SetRedBall(flag);
            UpdateEntitiesTrigger(flag);
        }

        private void UpdateEntitiesTrigger(bool value)
        {
            foreach (IEntityView view in _levelLoader.Entities)
            {
                view.BoxCollider2D.isTrigger = value;
            }
        }
    }
}