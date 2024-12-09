using System;

namespace GravityPong.Menu.Settings
{
    public interface ISettingsData : IService
    {
        Action<bool> OnPostProcessSettingsChanged { get; set; }
        Action<bool> OnCameraShakingSettingsChanged { get; set; }
        Action<int> OnVSyncSettingsChanged { get; set; }
        Action<int> OnTargetFramerateSettingsChanged { get; set; }

        bool PostProccesingEnabled { get; set; }
        bool CameraShaking { get; set; }
        int VSync { get; set; }
        int TargetFramerate { get; set; }

        void Save();
        void Load();
    }
}