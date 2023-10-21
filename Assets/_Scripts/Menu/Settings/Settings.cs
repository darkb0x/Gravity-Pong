using UnityEngine;
using UnityEngine.UI;

namespace GravityPong.Menu.Settings
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private Toggle PostPocessToggle;
        [SerializeField] private Toggle CameraShakingToggle;
        [Space]
        [SerializeField] private UIButton CloseSettingsButton;

        [Header("Panels")]
        [SerializeField] private UIPanelNavigator UIPanelNavigator;
        [SerializeField] private GameObject SettingViewObj;

        private ISettingsData _settingsData;

        private void Awake()
        {
            _settingsData = Services.Instance.Get<ISettingsData>();

            CloseSettingsButton.Initialize(CloseSettings);

            PostPocessToggle.onValueChanged.AddListener(SetPostProcessEnabled);
            CameraShakingToggle.onValueChanged.AddListener(SetCameraShakingEnabled);
        }
        private void Start()
        {
            _settingsData.Load();

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
            UIPanelNavigator.CloseAll();
        }
    }
}
