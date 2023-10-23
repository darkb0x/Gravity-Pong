using GravityPong.Infrasturcture;
using TMPro;
using UnityEngine;
using GravityPong.UI;
using GravityPong.Menu.Settings;

namespace GravityPong.Menu
{
    public class MainMenu : MonoBehaviour, IRequireEntryInitializing<UIPanelNavigator>
    {
        public const UIPanelID MAINMENU_PANEL_ID = UIPanelID.Menu_Main;

        [SerializeField] private TMP_Text HighscoreText;
        [Space]
        [SerializeField] private MenuButton SingleplayerButton;
        [SerializeField] private MenuButton MultiplayerButton;
        [SerializeField] private MenuButton SettingsButton;
        [SerializeField] private MenuButton QuitButton;

        private MenuButton[] _buttons;
        private UIPanelNavigator _uiPanelNavigator;

        public void Initialize(UIPanelNavigator uiPanelNavigator)
        {
            _uiPanelNavigator = uiPanelNavigator;

            InitializeButtons();
            InitializeHighscoreText();

            _uiPanelNavigator.Open(MAINMENU_PANEL_ID);
        }

        private void InitializeButtons()
        {
            _buttons = new MenuButton[4] { SingleplayerButton, MultiplayerButton, SettingsButton, QuitButton };

            foreach (var btn in _buttons)
                btn.Initialize(this);

            SingleplayerButton.Initialize(this, OpenSingleplayer);
            MultiplayerButton.Initialize(this, OpenMultiplayer);
            SettingsButton.Initialize(this,OpenSettings);
            QuitButton.Initialize(this, Quit);

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                QuitButton.gameObject.SetActive(false);
        }
        private void InitializeHighscoreText()
        {
            if (!PlayerPrefs.HasKey(Constants.PlayerPrefs.HIGHSCORE_PLAYERPREFS_KEY))
                PlayerPrefs.SetInt(Constants.PlayerPrefs.HIGHSCORE_PLAYERPREFS_KEY, 0);

            int highscore = PlayerPrefs.GetInt(Constants.PlayerPrefs.HIGHSCORE_PLAYERPREFS_KEY);
            HighscoreText.text = $"Highscore: {highscore}";
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
            SingleplayerButton.Button.interactable = false;
        }
        private void OpenMultiplayer()
        {
            Debug.Log("Kys");
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
