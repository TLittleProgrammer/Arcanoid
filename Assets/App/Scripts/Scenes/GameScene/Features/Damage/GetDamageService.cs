using App.Scripts.Scenes.GameScene.Features.Camera;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Shake;

namespace App.Scripts.Scenes.GameScene.Features.Damage
{
    public sealed class GetDamageService : IGetDamageService
    {
        private readonly IHealthContainer _healthContainer;
        private readonly IShakeService _shakeService;
        private readonly ICameraService _cameraService;

        public GetDamageService(
            IHealthContainer healthContainer,
            IShakeService shakeService, 
            ICameraService cameraService)
        {
            _healthContainer = healthContainer;
            _shakeService = shakeService;
            _cameraService = cameraService;
        }

        public void GetDamage(int damage)
        {
            _healthContainer.UpdateHealth(-damage);
            _shakeService.Shake(_cameraService.Camera.transform);
        }
    }
}