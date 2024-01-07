using GravityPong.Game.Singleplayer;
using System;
using UnityEngine;

namespace GravityPong.Game
{
    public interface IGameModeManager
    {
        void Initialize(CameraController camera, IGameHUD hud, Action leaveToMenuAction);
        void AddScore(float style, Transform ballInstance);
        void Restart();
        void StartTimer();
        void StopTimer();
        ScoreData CalculateScoreDataFromStyle(float style);
    }
}
