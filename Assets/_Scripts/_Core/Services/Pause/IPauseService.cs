using System;

namespace GravityPong.Pause
{
    public interface IPauseService : IService
    {
        Action<bool> OnPauseStateChanged { get; set; }
        bool PauseEnabled { get; set; }
        void Pause();
        void Resume();
    }
}