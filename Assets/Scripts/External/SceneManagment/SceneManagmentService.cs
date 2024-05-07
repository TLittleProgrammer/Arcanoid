using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace External.SceneManagment
{
    public class SceneManagementService : ISceneManagementService
    {
        private float _sceneLoadingProgress;
        
        public async UniTask LoadSceneAsync(string sceneName, Action sceneLoaded)
        {
            _sceneLoadingProgress = 0f;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncOperation.isDone)
            {
                _sceneLoadingProgress = asyncOperation.progress;
            }

            sceneLoaded?.Invoke();

            await UniTask.CompletedTask;
        }

        public float SceneLoadingProgress => _sceneLoadingProgress;
    }
}