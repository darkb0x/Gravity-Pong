using UnityEngine;
using UnityEngine.UI;
using GravityPong.Infrasturcture;
using GravityPong.UI;
using TMPro;

namespace GravityPong.Menu.Settings
{
    public class SettingsView : MonoBehaviour
    {
        public const UIPanelID SETTINGS_PANEL_ID = UIPanelID.Menu_Settings;
        private const float MIN_SCREEN_SCALE_SLIDER_VALUE = 0.35f;
        private const float MAX_SCREEN_SCALE_SLIDER_VALUE = 1f;
        private const int MIN_FPS = 30;
        private const int MAX_FPS = 320;

        [SerializeField] private bool DebugMode;
        [Space]
        [SerializeField] private Toggle PostPocessToggle;
        [SerializeField] private Toggle CameraShakingToggle;
        [SerializeField] private Slider ScreenScaleSlider;
        [SerializeField] private Toggle VSyncToggle;
        [SerializeField] private Slider TargetFramerateSlider;
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

            if (!DebugMode)
                ScreenScaleSlider.gameObject.SetActive(Application.isMobilePlatform && !(Application.platform != RuntimePlatform.WebGLPlayer));
        }

        private void InitializeUIActions()
        {
            CloseSettingsButton.Initialize(CloseSettings);
            ScreenScaleSlider.minValue = MIN_SCREEN_SCALE_SLIDER_VALUE;
            ScreenScaleSlider.maxValue = MAX_SCREEN_SCALE_SLIDER_VALUE;
            TargetFramerateSlider.minValue = MIN_FPS;
            TargetFramerateSlider.maxValue = MAX_FPS;

            PostPocessToggle.onValueChanged.AddListener(SetPostProcessEnabled);
            CameraShakingToggle.onValueChanged.AddListener(SetCameraShakingEnabled);
            ScreenScaleSlider.onValueChanged.AddListener(SetScreenScaleValue);
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
            ScreenScaleSlider.SetValueWithoutNotify(_settingsData.ScreenScale);
            VSyncToggle.SetIsOnWithoutNotify(_settingsData.VSync == 1);
            TargetFramerateSlider.SetValueWithoutNotify(_settingsData.TargetFramerate);
        }

        private void SetPostProcessEnabled(bool value)
        {
            _settingsData.PostProccesingEnabled = value;
        }
        private void SetCameraShakingEnabled(bool value)
        {
            _settingsData.CameraShaking = value;
        }
        private void SetScreenScaleValue(float value)
        {
            _settingsData.ScreenScale = value;
        }
        private void SetVSyncEnabled(bool value)
        {
            _settingsData.VSync = value ? 1 : 0;
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
