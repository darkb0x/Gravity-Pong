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

        [SerializeField] private MenuButton PlayButton;
        [SerializeField] private MenuButton SettingsButton;
        [SerializeField] private MenuButton QuitButton;

        private MenuButton[] _buttons;
        private UIPanelNavigator _uiPanelNavigator;

        public void Initialize(UIPanelNavigator uiPanelNavigator)
        {
            _uiPanelNavigator = uiPanelNavigator;

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            _buttons = new MenuButton[3] { PlayButton, SettingsButton, QuitButton };

            foreach (var btn in _buttons)
                btn.Initialize(this);

            PlayButton.Initialize(this, StartPlaying);
            SettingsButton.Initialize(this,OpenSettings);
            QuitButton.Initialize(this, Quit);

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                QuitButton.gameObject.SetActive(false);
        }

        public void Select(MenuButton button)
        {
            DeselectAll();
            button.Select();
        }
        private void DeselectAll()
        {
            foreach (var item in _buttons) item.Deselect();
        }

        private void StartPlaying()
        {
            _uiPanelNavigator.Open(UIPanelID.Menu_GameMode);
            DeselectAll();
        }
        private void OpenSettings()
        {
            _uiPanelNavigator.Open(SettingsView.SETTINGS_PANEL_ID);
            DeselectAll();
        }
        private void Quit()
        {
            Application.Quit();
            DeselectAll();
        }
    }
}
