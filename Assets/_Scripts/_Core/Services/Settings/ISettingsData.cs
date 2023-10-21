using System;

namespace GravityPong.Menu.Settings
{
    public interface ISettingsData : IService
    {
        Action<bool> OnPostProcessSettingsChanges { get; set; }
        Action<bool> OnCameraShakingSettingsChanges { get; set; }

        bool PostProccesingEnabled { get; set; }
        bool CameraShaking { get; set; }

        void Save();
        void Load();
    }
}