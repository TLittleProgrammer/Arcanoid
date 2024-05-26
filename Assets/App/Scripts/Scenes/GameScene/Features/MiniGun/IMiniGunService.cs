using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.MiniGun
{
    public interface IMiniGunService : IActivable
    {
        bool ActiveMiniGun { get; set; }
        void RecalculateSpawnPositions();
    }
}