using GravityPong.Game.Singleplayer.Player;
using GravityPong.Infrasturcture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GravityPong.Game.Singleplayer
{
    public class SingleplayerEntryPoint : SceneEntryPoint
    {
        [SerializeField] private CameraController CameraController;
        [SerializeField] private SingleplayerGameManager GameManager;
        [SerializeField] private PlayerContoller Player;
        [SerializeField] private Ball.Ball Ball;

        protected override void InitializeEntries()
        {
            GameManager.Initialize(CameraController);
            Player.Initialize();
            Ball.Initialize();
        }
    }
}
