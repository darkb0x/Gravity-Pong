using GravityPong.Menu.Settings;
using GravityPong.Pause;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GravityPong.Infrasturcture
{
    public class GameEntryPoint : MonoBehaviour, ICoroutineRunner
    {
        public static GameEntryPoint Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }    

            Instance = this;

            RegisterServices();

            DontDestroyOnLoad(this);

            SceneManager.activeSceneChanged += OnSceneLoaded;
        }


        private void RegisterServices()
        {
            Services _services = new Services();

            _services.Register<IInputService>(GetInputService());
            _services.Register<ICoroutineRunner>(this);
            _services.Register<IAudioService>(new AudioService(gameObject, _services.Get<ICoroutineRunner>()));
            _services.Register<ISceneLoader>(new SceneLoader(_services.Get<ICoroutineRunner>()));
            _services.Register<ISettingsData>(new SettingsData());
            _services.Register<IPauseService>(new PauseService());
        }

        private IInputService GetInputService()
        {
            if (Application.isMobilePlatform)
                return new MobileInputService();

            return new PCInputService();
        }
        private void InitializeSceneEntryPoint()
        {
            SceneEntryPoint sceneEntryPoint = FindObjectOfType<SceneEntryPoint>();
            if (sceneEntryPoint != null)
            {
                sceneEntryPoint.StartInitializing();
            }
        }
        private void OnSceneLoaded(Scene arg0, Scene arg1)
        {
            InitializeSceneEntryPoint();
        }

        #region ICoroutineRunner
        public Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
        #endregion
    }
}