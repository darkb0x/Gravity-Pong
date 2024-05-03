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
        [SerializeField] private ScreenColorController _screenColorController;
        [SerializeField] private SingleplayerGameManager GameManager;
        [SerializeField] private SingleplayerGameHUD GameHUD;
        [SerializeField] private PlayerContoller Player;
        [SerializeField] private Ball.Ball Ball;

        protected override void InitializeEntries()
        {
            GameManager.Initialize(CameraController, GameHUD, 
                () => Services.Instance.Get<ISceneLoader>().LoadScene(Constants.Scenes.MAIN_MENU_SCENE_NAME));
            Player.Initialize();
            Ball.Initialize(GameManager);
            _screenColorController.Initialize(GameManager, Camera.main);
        }
    }
}
