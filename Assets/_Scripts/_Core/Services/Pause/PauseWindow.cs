using UnityEngine;
using System;
using TMPro;

namespace GravityPong.Pause
{
    public class PauseWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _highscoreText;
        [Space]
        [SerializeField] private Menu.UIButton ContinueButton;
        [SerializeField] private Menu.UIButton MenuButton;

        private IPauseService _pauseService;
        private bool _isOpened;

        public void Initialize(Action menuButtonAction)
        {
            _pauseService = Services.Instance.Get<IPauseService>();

            ContinueButton.Initialize(Close);
            MenuButton.Initialize(menuButtonAction);
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

        // I was forced to use Time.timeScale because of a bug
        public void Open()
        {
            _isOpened = true;
            _pauseService.Pause();
            Time.timeScale = 0f;

            _highscoreText.text = "Highscore: " + PlayerPrefs.GetInt(Constants.PlayerPrefs.HIGHSCORE_KEY);

            gameObject.SetActive(true);
        }
        public void Close()
        {
            _isOpened = false;
            _pauseService.Resume();
            Time.timeScale = 1f;

            gameObject.SetActive(false);
            ContinueButton.Deselect();
        }
    }
}