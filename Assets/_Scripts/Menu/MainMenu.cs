using GravityPong.Infrasturcture;
using TMPro;
using UnityEngine;
using GravityPong.UI;
using GravityPong.Menu.Settings;

namespace GravityPong.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public const UIPanelID MAINMENU_PANEL_ID = UIPanelID.Menu_Main;

        [SerializeField] private TMP_Text HighscoreText;
        [Space]
        [SerializeField] private MenuButton PlayButton;
        [SerializeField] private MenuButton SettingsButton;
        [SerializeField] private MenuButton QuitButton;

        private MenuButton[] _buttons;
        private UIPanelNavigator _uiPanelNavigator;

        public void Initialize(UIPanelNavigator uiPanelNavigator)
        {
            _uiPanelNavigator = uiPanelNavigator;

            InitializePlayerPrefsSave();
            InitializeButtons();
            InitializeHighscoreText();

            _uiPanelNavigator.Open(MAINMENU_PANEL_ID);
        }

        private void InitializeButtons()
        {
            _buttons = new MenuButton[3] { PlayButton, SettingsButton, QuitButton };

            foreach (var btn in _buttons)
                btn.Initialize(this);

            PlayButton.Initialize(this, OpenSingleplayer);
            SettingsButton.Initialize(this,OpenSettings);
            QuitButton.Initialize(this, Quit);

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                QuitButton.gameObject.SetActive(false);
        }
        private void InitializeHighscoreText()
        {
            int highscore = PlayerPrefs.GetInt(Constants.PlayerPrefs.HIGHSCORE_KEY);
            HighscoreText.text = $"Highscore: {highscore}";
        }

        private static void InitializePlayerPrefsSave()
        {
            if (!PlayerPrefs.HasKey(Constants.PlayerPrefs.HIGHSCORE_KEY))
                PlayerPrefs.SetInt(Constants.PlayerPrefs.HIGHSCORE_KEY, 0);
            if (!PlayerPrefs.HasKey(Constants.PlayerPrefs.RECORD_OF_HITS_KEY))
                PlayerPrefs.SetInt(Constants.PlayerPrefs.RECORD_OF_HITS_KEY, 0);
            if (!PlayerPrefs.HasKey(Constants.PlayerPrefs.RECORD_OF_TIME_KEY))
                PlayerPrefs.SetFloat(Constants.PlayerPrefs.RECORD_OF_TIME_KEY, 0);

            PlayerPrefs.Save();
        }

        public void Select(MenuButton button)
        {
            foreach (var btn in _buttons)
            {
                btn.Deselect();
            }
            button.Select();
        }

        private void OpenSingleplayer()
        {
            Services.Instance.Get<ISceneLoader>().LoadScene(Constants.Scenes.SINGLEPLAYER_SCENE_NAME);
            PlayButton.Button.interactable = false;
        }
        private void OpenSettings()
        {
            _uiPanelNavigator.Open(SettingsView.SETTINGS_PANEL_ID);
        }
        private void Quit()
        {
            Application.Quit();
        }
    }
}
