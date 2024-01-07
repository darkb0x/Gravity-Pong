using GravityPong.Game.Singleplayer;
using GravityPong.Game.Singleplayer.Ball;
using GravityPong.Pause;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GravityPong.Game.Arcade
{
    public class ArcadeGameManager : MonoBehaviour, IGameModeManager
    {
        private const float START_STYLE_VALUE = 0.3f;

        [Header("HUD")]
        [SerializeField] private Stylemeter Stylemeter;

        [Header("Audio")]
        [SerializeField] private AudioClip BounceSoundPerfectStart;
        [SerializeField] private AudioClip BounceSoundPerfectEnd;

        private IGameHUD _hud;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                _hud.UpdateScoreText(_score, _previousHighscore);
            }
        }

        private IAudioService _audioService;
        private IPauseService _pauseService;

        private CameraController _camera;

        private int _score;

        private int _previousHighscore;
        private int _maxStyleStreak;

        public void Initialize(CameraController camera, IGameHUD hud, Action leaveToMenuAction)
        {
            _camera = camera;
            _hud = hud;

            _audioService = Services.Instance.Get<IAudioService>();
            _pauseService = Services.Instance.Get<IPauseService>();

            _previousHighscore = PlayerPrefs.GetInt(Constants.PlayerPrefs.ARCADE_HIGHSCORE_KEY);

            Score = 0;

            _hud.Initialize(leaveToMenuAction);
            Stylemeter.Initialize(START_STYLE_VALUE);

            _hud.ClosePause();
        }

        private void Update()
        {
            if (!_pauseService.PauseEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _hud.OpenPause();
                }
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!Application.isEditor && !focus && !_pauseService.PauseEnabled)
                _hud.OpenPause();
        }

        public void Restart()
        {
            if (Score > _previousHighscore)
            {
                PlayerPrefs.SetInt(Constants.PlayerPrefs.ARCADE_HIGHSCORE_KEY, Score);
                _previousHighscore = PlayerPrefs.GetInt(Constants.PlayerPrefs.ARCADE_HIGHSCORE_KEY);
            }

            PlayerPrefs.Save();

            _camera.Shake(new Vector4(-10, 10, -4, 4), 1f, 0.7f);

            Score = 0;
            _maxStyleStreak = 0;

            Stylemeter.ResetStyle(START_STYLE_VALUE);
        }

        public void StartTimer() { return; }

        public void StopTimer() { return; }

        public void AddScore(float style, Transform ballInstance)
        {
            ScoreData data = CalculateScoreDataFromStyle(style);

            Score += data.Score;

            Stylemeter.AddStyle(data);

            // 'slow-motion'
            if (data.Style == ScoreData.MAX_STYLE)
            {
                _audioService.PlaySound(BounceSoundPerfectStart, transform);
                _pauseService.Pause();
                _camera.FocusOn(ballInstance, 5f, 0.9f, () =>
                {
                    _audioService.PlaySound(BounceSoundPerfectEnd, transform);
                    _pauseService.Resume();

                    var particle = BallParticlesController.Particles.Get();
                    particle.Play(ballInstance.position, Quaternion.identity, false);
                });

                _maxStyleStreak++;

                if (_maxStyleStreak >= 2)
                    _hud.ShowStreak(_maxStyleStreak);
            }
            else
            {
                _maxStyleStreak = 0;
            }
        }

        public ScoreData CalculateScoreDataFromStyle(float style)
        {
            int score;
            string styleMessage;

            switch (style)
            {
                case 0.25f:
                    score = 1;
                    styleMessage = "center";
                    break;
                case 0.5f:
                    score = 3;
                    styleMessage = "side";
                    break;
                case 0.75f:
                    score = 5;
                    styleMessage = "edge";
                    break;
                case 1f:
                    score = 20;
                    styleMessage = "vertical";
                    break;
                default:
                    goto case 0.25f;
            }

            return new ScoreData(score, style, styleMessage);
        }
    }
}
