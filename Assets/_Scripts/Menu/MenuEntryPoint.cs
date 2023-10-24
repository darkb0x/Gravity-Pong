using UnityEngine;
using GravityPong.UI;
using GravityPong.Menu.Settings;
using System.Collections.Generic;
using GravityPong.Infrasturcture;

namespace GravityPong.Menu
{
    public class MenuEntryPoint : SceneEntryPoint
    {
        [SerializeField] private GameObject MainMenuViewObj;
        [SerializeField] private GameObject SettingsViewObj;
        [SerializeField] private GameObject MultiplayerViewObj;
        [Space]
        [SerializeField] private MainMenu Menu;
        [SerializeField] private SettingsView Settings;
        [SerializeField] private MultiplayerView Multiplayer;

        protected override void InitializeEntries()
        {
            Dictionary<UIPanelID, GameObject> uiPanels = new Dictionary<UIPanelID, GameObject>() 
            {
                [UIPanelID.Menu_Main] = MainMenuViewObj,
                [UIPanelID.Menu_Settings] = SettingsViewObj,
                [UIPanelID.Menu_Multiplayer] = MultiplayerViewObj,
            };
            UIPanelNavigator uiPanelNavigator = new UIPanelNavigator(MainMenuViewObj, uiPanels);

            Menu.Initialize(uiPanelNavigator);
            Settings.Initialize(uiPanelNavigator, Services.Instance.Get<ISettingsData>());
            Multiplayer.Initialize(uiPanelNavigator);
        }
    }
}