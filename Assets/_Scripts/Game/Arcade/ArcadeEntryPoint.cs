using GravityPong.Game.Singleplayer.Ball;
using GravityPong.Game.Singleplayer.Player;
using GravityPong.Game.Singleplayer;
using UnityEngine;
using GravityPong.Infrasturcture;

namespace GravityPong.Game.Arcade
{
    public class ArcadeEntryPoint : SceneEntryPoint
    {
        [SerializeField] private CameraController CameraController;
        [SerializeField] private ArcadeGameManager GameManager;
        [SerializeField] private ArcadeGameHUD GameHUD;
        [SerializeField] private PlayerContoller Player;
        [SerializeField] private Ball Ball;

        protected override void InitializeEntries()
        {
            GameManager.Initialize(CameraController, GameHUD,
                () => Services.Instance.Get<ISceneLoader>().LoadScene(Constants.Scenes.SINGLEPLAYER_SCENE_NAME));
            Player.Initialize();
            Ball.Initialize(GameManager);
        }
    }
}
