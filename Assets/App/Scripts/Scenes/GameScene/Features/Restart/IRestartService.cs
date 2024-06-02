namespace App.Scripts.Scenes.GameScene.Features.Restart
{
    public interface IRestartService
    {
        void TryRestartLevel();
        void RestartSession();
    }
}