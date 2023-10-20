using System;

namespace GravityPong.Pause
{
    public class PauseService : IPauseService
    {
        private Action<bool> m_OnPauseStateChanged;

        public Action<bool> OnPauseStateChanged { get => m_OnPauseStateChanged; set => m_OnPauseStateChanged = value; }

        public bool PauseEnabled
        {
            get => _pauseEnabled;
            set
            {
                _pauseEnabled = value;
                OnPauseStateChanged?.Invoke(_pauseEnabled);
            }
        }

        private bool _pauseEnabled;

        public void Pause()
        {
            PauseEnabled = true;
        }

        public void Resume()
        {
            PauseEnabled = false;
        }
    }
}