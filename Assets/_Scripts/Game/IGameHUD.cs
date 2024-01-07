using System;

namespace GravityPong.Game
{
    public interface IGameHUD
    {
        void Initialize(Action leaveToMenu);
        void UpdateScoreText(int score, int previousHighscore);
        void ClosePause();
        void OpenPause();
        void ShowStreak(int value);
    }
}
