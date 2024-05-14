using App.Scripts.Scenes.GameScene.Infrastructure;

namespace App.Scripts.Scenes.GameScene.LevelProgress
{
    public interface IStopGameService : IRestartable
    {
        void Stop();
    }
}