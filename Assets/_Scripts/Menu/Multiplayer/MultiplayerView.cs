using GravityPong.Infrasturcture;
using GravityPong.UI;
using UnityEngine;
using Mirror;
using System;
using TMPro;

namespace GravityPong.Menu
{
    public class MultiplayerView : MonoBehaviour, IRequireEntryInitializing<UIPanelNavigator>
    {
        public const UIPanelID MULTUPLAYER_PANEL_ID = UIPanelID.Menu_Multiplayer;

        [Serializable]
        private struct IPPanelView
        {
            public GameObject Panel;
            [Space]
            public UIButton ConfirmButton;
            public UIButton CancelButton;
            [Space]
            public TMP_InputField IPInputField;
        } 

        [Header("Main")]
        [SerializeField] private UIButton CreateButton;
        [SerializeField] private UIButton ConnectButton;
        [SerializeField] private UIButton CloseMultiplayerButton;

        [Header("Server IP Panel")]
        [SerializeField] private IPPanelView IPPanel;

        private UIPanelNavigator _uiPanelNavigator;

        public void Initialize(UIPanelNavigator uiPanelNavigator)
        {
            _uiPanelNavigator = uiPanelNavigator;

            InitializeButtons();

            CloseIPPanel();
        }

        private void InitializeButtons()
        {
            CreateButton.Initialize(CreateHost);
            ConnectButton.Initialize(OpenIPPanel);
            CloseMultiplayerButton.Initialize(CloseMultiplayer);

            IPPanel.ConfirmButton.Initialize(ConnectToHost);
            IPPanel.CancelButton.Initialize(CloseIPPanel);
            IPPanel.IPInputField.onValueChanged.AddListener(SetIPAdress);
        }

        private void CreateHost()
        {
            NetworkManager.singleton.StartHost();
        }
        private void ConnectToHost()
        {
            NetworkManager.singleton.StartClient();
        }
        private void OpenIPPanel()
        {
            IPPanel.Panel.SetActive(true);
            CloseMultiplayerButton.gameObject.SetActive(false);
        }
        private void CloseIPPanel()
        {
            IPPanel.Panel.SetActive(false);
            CloseMultiplayerButton.gameObject.SetActive(true);
        }
        private void SetIPAdress(string value)
        {
            NetworkManager.singleton.networkAddress = value;

            if(string.IsNullOrEmpty(value))
            {
                IPPanel.ConfirmButton.Button.interactable = false;
            }
            else
            {
                IPPanel.ConfirmButton.Button.interactable = true;
            }
        }
        private void CloseMultiplayer()
        {
            CloseIPPanel();
            _uiPanelNavigator.CloseAll();
        }
    }
}
