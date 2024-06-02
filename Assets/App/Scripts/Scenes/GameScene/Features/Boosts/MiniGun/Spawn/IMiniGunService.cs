using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Spawn
{
    public interface IMiniGunService : IActivable
    {
        bool ActiveMiniGun { get; set; }
    }
}