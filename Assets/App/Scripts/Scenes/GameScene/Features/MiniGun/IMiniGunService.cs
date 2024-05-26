namespace App.Scripts.Scenes.GameScene.Features.MiniGun
{
    public interface IMiniGunService
    {
        bool IsActive { get; set; }
        void RecalculateSpawnPositions();
    }
}