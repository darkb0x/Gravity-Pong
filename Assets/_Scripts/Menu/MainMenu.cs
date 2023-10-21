using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GravityPong.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text HighscoreText;
        [Space]
        [SerializeField] private MenuButton SingleplayerButton;
        [SerializeField] private MenuButton MultiplayerButton;
        [SerializeField] private MenuButton SettingsButton;
        [SerializeField] private MenuButton QuitButton;

        [Header("Panels")]
        [SerializeField] private UIPanelNavigator UIPanelNavigator;
        [SerializeField] private GameObject MainMenuViewObj;
        [SerializeField] private GameObject SettingsViewObj;

        private MenuButton[] _buttons;

        private void Awake()
        {
            UIPanelNavigator.Initialize(MainMenuViewObj, new GameObject[2] { MainMenuViewObj, SettingsViewObj });

            InitializeButtons();
            InitializeHighscoreText();
        }
        private void Start()
        {
            UIPanelNavigator.Open(MainMenuViewObj);
        }

        private void InitializeButtons()
        {
            _buttons = new MenuButton[4] { SingleplayerButton, MultiplayerButton, SettingsButton, QuitButton };

            foreach (var btn in _buttons)
                btn.Initialize(this);

            SingleplayerButton.Initialize(this, Singleplayer);
            MultiplayerButton.Initialize(this, Multiplayer);
            SettingsButton.Initialize(this,Settings);
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

        private void Singleplayer()
        {
            Services.Instance.Get<ISceneLoader>().LoadScene(Constants.Scenes.SINGLEPLAYER_SCENE_NAME);
            SingleplayerButton.Button.interactable = false;
        }
        private void Multiplayer()
        {
            Debug.Log("Kys");
        }
        private void Settings()
        {
            UIPanelNavigator.Open(SettingsViewObj);
        }
        private void Quit()
        {
            Application.Quit();
        }
    }
}
