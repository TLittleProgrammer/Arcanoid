using System;
using Cysharp.Threading.Tasks;

namespace External.SceneManagment
{
    public interface ISceneManagementService
    {
        float SceneLoadingProgress { get; }
        UniTask LoadSceneAsync(string sceneName, Action sceneLoaded);
    }
}