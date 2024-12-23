using System;
using UnityEngine;

namespace GravityPong.Menu.Settings
{
    public class SettingsData : ISettingsData
    {
        [Serializable]
        public struct SettingsSaveData
        {
            public bool PostProcessEnabled;
            public bool CameraShakingEnabled;
            public int VSync;
            public int TargetFramerate;

            public SettingsSaveData(bool postProcessEnabled, bool cameraShakingEnabled, int vsync, int targetFramerate)
            {
                PostProcessEnabled = postProcessEnabled;
                CameraShakingEnabled = cameraShakingEnabled;
                VSync = vsync;
                TargetFramerate = targetFramerate;
            }
        }

        public Action<bool> OnPostProcessSettingsChanged { get; set; }
        public Action<bool> OnCameraShakingSettingsChanged { get; set; }
        public Action<int> OnVSyncSettingsChanged { get; set; }
        public Action<int> OnTargetFramerateSettingsChanged { get; set; }

        public bool PostProccesingEnabled
        {
            get => _postProcessEnabled;
            set
            {
                _postProcessEnabled = value;
                OnPostProcessSettingsChanged?.Invoke(_postProcessEnabled);
            }
        }
        public bool CameraShaking {
            get => _cameraShakingEnabled;
            set
            {
                _cameraShakingEnabled = value;
                OnCameraShakingSettingsChanged?.Invoke(_cameraShakingEnabled);
            }
        }
        public int VSync
        {
            get => _vsync;
            set
            {
                _vsync = value;
                OnVSyncSettingsChanged?.Invoke(_vsync);
            }
        }
        public int TargetFramerate
        {
            get => _targetFramerate;
            set
            {
                _targetFramerate = Mathf.Clamp(value, 30, 320);
                OnTargetFramerateSettingsChanged?.Invoke(_targetFramerate);
            }
        }

        private bool _postProcessEnabled;
        private bool _cameraShakingEnabled;
        private int _vsync;
        private int _targetFramerate;

        public SettingsData()
        {
            if (Application.isMobilePlatform)
                _postProcessEnabled = false;
            else
                _postProcessEnabled = true;

            _cameraShakingEnabled = true;
            _vsync = 1;
            _targetFramerate = 60;

            // Events
            OnPostProcessSettingsChanged += _ => Save();
            OnCameraShakingSettingsChanged += _ => Save();
            OnVSyncSettingsChanged += _ => Save();
            OnTargetFramerateSettingsChanged += _ => Save();
        }

        public void Save()
        {
            var saveData = new SettingsSaveData(
                _postProcessEnabled,
                _cameraShakingEnabled,
                _vsync,
                _targetFramerate
                );

            PlayerPrefs.SetString(Constants.PlayerPrefs.SETTINGS_KEY, JsonUtility.ToJson(saveData));
        }

        public void Load()
        {
            if(!PlayerPrefs.HasKey(Constants.PlayerPrefs.SETTINGS_KEY))
            {
                Save();
                return;
            }

            var saveData = JsonUtility.FromJson<SettingsSaveData>(PlayerPrefs.GetString(Constants.PlayerPrefs.SETTINGS_KEY));
            PostProccesingEnabled = saveData.PostProcessEnabled;
            CameraShaking = saveData.CameraShakingEnabled;
            VSync = saveData.VSync;
            TargetFramerate = saveData.TargetFramerate;
        }
    }
}