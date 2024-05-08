using Cysharp.Threading.Tasks;

namespace App.Scripts.External.SceneManagment
{
    public interface ISceneManagementService
    {
        float SceneLoadingProgress { get; }
        UniTask LoadSceneAsync(string sceneName);
    }
}