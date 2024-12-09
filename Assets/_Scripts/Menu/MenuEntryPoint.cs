using UnityEngine;
using GravityPong.UI;
using GravityPong.Menu.Settings;
using System.Collections.Generic;
using GravityPong.Infrasturcture;

namespace GravityPong.Menu
{
    public class MenuEntryPoint : SceneEntryPoint
    {
        [SerializeField] private GameObject _mainMenuViewObj;
        [SerializeField] private GameObject _settingsViewObj;
        [SerializeField] private GameObject _gameModeViewObj;
        [Space]
        [SerializeField] private MainMenu _menu;
        [SerializeField] private SettingsView _settings;
        [SerializeField] private GameModeView _gameMode;

        protected override void InitializeEntries()
        {
            Dictionary<UIPanelID, GameObject> uiPanels = new Dictionary<UIPanelID, GameObject>()
            {
                [UIPanelID.Menu_Main] = _mainMenuViewObj,
                [UIPanelID.Menu_Settings] = _settingsViewObj,
                [UIPanelID.Menu_GameMode] = _gameModeViewObj,
            };
            UIPanelNavigator uiPanelNavigator = new UIPanelNavigator(_mainMenuViewObj, uiPanels);

            _menu.Initialize(uiPanelNavigator);
            _settings.Initialize(uiPanelNavigator, Services.Instance.Get<ISettingsData>());
            _gameMode.Initialize(uiPanelNavigator, Services.Instance.Get<ISceneLoader>(), Services.Instance.Get<IGameSaveDataController>());

            uiPanelNavigator.Open(UIPanelID.Menu_Main);
        }
    }
}