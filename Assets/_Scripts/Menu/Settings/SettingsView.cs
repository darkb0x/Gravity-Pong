using UnityEngine;
using UnityEngine.UI;
using GravityPong.UI;
using TMPro;

namespace GravityPong.Menu.Settings
{
    public class SettingsView : MonoBehaviour
    {
        public const UIPanelID SETTINGS_PANEL_ID = UIPanelID.Menu_Settings;
        private const int MIN_FPS = 30;
        private const int MAX_FPS = 320;

        [SerializeField] private bool DebugMode;
        [Space]
        [SerializeField] private Toggle PostPocessToggle;
        [SerializeField] private Toggle CameraShakingToggle;
        [SerializeField] private Toggle VSyncToggle;
        [SerializeField] private Slider TargetFramerateSlider;
        [SerializeField] private GameObject _targetFramerateGameObj;
        [SerializeField] private TMP_Text CurrentMaxFPS_Text;
        [Space]
        [SerializeField] private UIButton CloseSettingsButton;

        private UIPanelNavigator _uiPanelNavigator;
        private ISettingsData _settingsData;

        public void Initialize(UIPanelNavigator uiPanelNavigator, ISettingsData settingsData)
        {
            _uiPanelNavigator = uiPanelNavigator;
            _settingsData = settingsData;
            _settingsData.Load();

            InitializeUIActions();
            LoadSaveData();

            if(!Application.isEditor)
                DebugMode = false;
        }

        private void InitializeUIActions()
        {
            CloseSettingsButton.Initialize(CloseSettings);
            TargetFramerateSlider.minValue = MIN_FPS;
            TargetFramerateSlider.maxValue = MAX_FPS;

            PostPocessToggle.onValueChanged.AddListener(SetPostProcessEnabled);
            CameraShakingToggle.onValueChanged.AddListener(SetCameraShakingEnabled);
            VSyncToggle.onValueChanged.AddListener(SetVSyncEnabled);
            TargetFramerateSlider.onValueChanged.AddListener(
                new UnityEngine.Events.UnityAction<float>(value => SetTargetFramerateValue((int) value))
                );
            CurrentMaxFPS_Text.text = _settingsData.TargetFramerate.ToString();
        }
        private void LoadSaveData()
        {
            PostPocessToggle.SetIsOnWithoutNotify(_settingsData.PostProccesingEnabled);
            CameraShakingToggle.SetIsOnWithoutNotify(_settingsData.CameraShaking);
            VSyncToggle.SetIsOnWithoutNotify(_settingsData.VSync == 1);
            TargetFramerateSlider.SetValueWithoutNotify(_settingsData.TargetFramerate);

            SetVSyncEnabled(_settingsData.VSync == 1);
        }

        private void SetPostProcessEnabled(bool value)
        {
            _settingsData.PostProccesingEnabled = value;
        }
        private void SetCameraShakingEnabled(bool value)
        {
            _settingsData.CameraShaking = value;
        }
        private void SetVSyncEnabled(bool value)
        {
            _settingsData.VSync = value ? 1 : 0;
            _targetFramerateGameObj.SetActive(!value);
        }
        private void SetTargetFramerateValue(int value)
        {
            _settingsData.TargetFramerate = value;
            CurrentMaxFPS_Text.text = _settingsData.TargetFramerate.ToString();
        }

        private void CloseSettings()
        {
            _uiPanelNavigator.CloseAll();
        }
    }
}
