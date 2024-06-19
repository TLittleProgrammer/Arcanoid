using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Restart
{
    public interface IUpdateServiceForNewLevel
    {
        UniTask Update();
    }
}