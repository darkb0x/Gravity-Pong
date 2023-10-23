using UnityEngine;
using UnityEngine.UI;
using GravityPong.Infrasturcture;
using GravityPong.UI;

namespace GravityPong.Menu.Settings
{
    public class SettingsView : MonoBehaviour, IRequireEntryInitializing<UIPanelNavigator, ISettingsData>
    {
        public const UIPanelID SETTINGS_PANEL_ID = UIPanelID.Menu_Settings;

        [SerializeField] private Toggle PostPocessToggle;
        [SerializeField] private Toggle CameraShakingToggle;
        [Space]
        [SerializeField] private UIButton CloseSettingsButton;

        private UIPanelNavigator _uiPanelNavigator;
        private ISettingsData _settingsData;

        public void Initialize(UIPanelNavigator uiPanelNavigator, ISettingsData settingsData)
        {
            _uiPanelNavigator = uiPanelNavigator;
            _settingsData = settingsData;
            _settingsData.Load();

            CloseSettingsButton.Initialize(CloseSettings);

            PostPocessToggle.onValueChanged.AddListener(SetPostProcessEnabled);
            CameraShakingToggle.onValueChanged.AddListener(SetCameraShakingEnabled);

            PostPocessToggle.SetIsOnWithoutNotify(_settingsData.PostProccesingEnabled);
            CameraShakingToggle.SetIsOnWithoutNotify(_settingsData.CameraShaking);
        }

        private void SetPostProcessEnabled(bool value)
        {
            _settingsData.PostProccesingEnabled = value;
        }
        private void SetCameraShakingEnabled(bool value)
        {
            _settingsData.CameraShaking = value;
        }

        private void CloseSettings()
        {
            _uiPanelNavigator.CloseAll();
        }
    }
}
