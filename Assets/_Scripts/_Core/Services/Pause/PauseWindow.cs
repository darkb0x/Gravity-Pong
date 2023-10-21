using UnityEngine;

namespace GravityPong.Pause
{
    public class PauseWindow : MonoBehaviour
    {
        [SerializeField] private Menu.UIButton ResumeButton;

        private IPauseService _pauseService;
        private bool _isOpened;

        public void Initialize()
        {
            _pauseService = Services.Instance.Get<IPauseService>();

            ResumeButton.Initialize(Close);
        }

        private void Update()
        {
            if (!_isOpened)
                return;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }

        public void Open()
        {
            _isOpened = true;
            _pauseService.Pause();
            Time.timeScale = 0f;

            gameObject.SetActive(true);
        }
        public void Close()
        {
            _isOpened = false;
            _pauseService.Resume();
            Time.timeScale = 1f;

            gameObject.SetActive(false);
            ResumeButton.Deselect();
        }
    }
}