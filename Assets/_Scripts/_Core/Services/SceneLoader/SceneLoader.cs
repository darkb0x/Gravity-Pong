using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Object = UnityEngine.Object;

namespace GravityPong
{
    public class SceneLoader : ISceneLoader
    {
        private const string LOADING_SCREEN_PREFAB_PATH = "Prefabs/LoadingScreen";

        private readonly ICoroutineRunner _coroutineRunner;

        private LoadingScreen _loadingScreen;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;

            InitializeLoadingScreen();
        }

        private void InitializeLoadingScreen()
        {
            var prefab = Resources.Load<LoadingScreen>(LOADING_SCREEN_PREFAB_PATH);
            _loadingScreen = Object.Instantiate(prefab);
            Object.DontDestroyOnLoad(_loadingScreen);
            _loadingScreen.gameObject.SetActive(false);
        }

        public void LoadScene(string name, Action onLoaded = null)
        {
            _loadingScreen.Show(() => _coroutineRunner.RunCoroutine(LoadSceneCoroutine(name, onLoaded)));
        }

        private IEnumerator LoadSceneCoroutine(string name, Action onLoaded)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(name);

            while (!asyncOperation.isDone)
                yield return null;

            onLoaded?.Invoke();
            _loadingScreen.Hide();
        }
    }
}