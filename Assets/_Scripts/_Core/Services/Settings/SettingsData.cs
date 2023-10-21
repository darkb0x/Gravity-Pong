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

            public SettingsSaveData(bool postProcessEnabled, bool cameraShakingEnabled)
            {
                PostProcessEnabled = postProcessEnabled;
                CameraShakingEnabled = cameraShakingEnabled;
            }
        }

        public Action<bool> OnPostProcessSettingsChanges { get; set; }
        public Action<bool> OnCameraShakingSettingsChanges { get; set; }

        public bool PostProccesingEnabled
        {
            get => _postProcessEnabled;
            set
            {
                _postProcessEnabled = value;
                OnPostProcessSettingsChanges?.Invoke(_postProcessEnabled);
            }
        }
        public bool CameraShaking {
            get => _cameraShakingEnabled;
            set
            {
                _cameraShakingEnabled = value;
                OnCameraShakingSettingsChanges?.Invoke(_cameraShakingEnabled);
            }
        }

        private bool _postProcessEnabled;
        private bool _cameraShakingEnabled;

        public SettingsData()
        {
            if (Application.isMobilePlatform)
                _postProcessEnabled = false;
            else
                _postProcessEnabled = true;

            _cameraShakingEnabled = true;

            // Events
            OnPostProcessSettingsChanges += _ => Save();
            OnCameraShakingSettingsChanges += _ => Save();
        }

        public void Save()
        {
            var saveData = new SettingsSaveData(_postProcessEnabled, _cameraShakingEnabled);
            PlayerPrefs.SetString(Constants.PlayerPrefs.SETTINGS_PLAYERPREFS_KEY, JsonUtility.ToJson(saveData));
        }

        public void Load()
        {
            if(!PlayerPrefs.HasKey(Constants.PlayerPrefs.SETTINGS_PLAYERPREFS_KEY))
            {
                Save();
                return;
            }

            var saveData = JsonUtility.FromJson<SettingsSaveData>(PlayerPrefs.GetString(Constants.PlayerPrefs.SETTINGS_PLAYERPREFS_KEY));
            PostProccesingEnabled = saveData.PostProcessEnabled;
            CameraShaking = saveData.CameraShakingEnabled;
        }
    }
}