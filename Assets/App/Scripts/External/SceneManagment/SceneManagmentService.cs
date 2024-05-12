using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Scripts.External.SceneManagment
{
    public class SceneManagementService : ISceneManagementService
    {
        private float _sceneLoadingProgress;
        
        public async UniTask LoadSceneAsync(string sceneName)
        {
            _sceneLoadingProgress = 0f;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncOperation.isDone)
            {
                _sceneLoadingProgress = asyncOperation.progress;
                await UniTask.Yield();
            }
        }

        public float SceneLoadingProgress => _sceneLoadingProgress;
    }
}