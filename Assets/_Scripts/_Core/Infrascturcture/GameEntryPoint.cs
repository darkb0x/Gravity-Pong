using GravityPong.Menu.Settings;
using GravityPong.Pause;
using System.Collections;
using UnityEngine;

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

        #region ICoroutineRunner
        public Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }
        #endregion
    }
}