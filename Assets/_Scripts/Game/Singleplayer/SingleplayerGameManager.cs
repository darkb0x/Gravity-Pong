using UnityEngine;
using TMPro;
using GravityPong.Pause;
using GravityPong.Game.Singleplayer.Ball;
using System;

namespace GravityPong.Game.Singleplayer
{
    public class SingleplayerGameManager : MonoBehaviour, IGameModeManager
    {
        [Header("HUD")]
        [SerializeField] private Stylemeter Stylemeter;

        [Header("Audio")]
        [SerializeField] private AudioClip BounceSoundPerfectStart;
        [SerializeField] private AudioClip BounceSoundPerfectEnd;

        public Action<int> OnScoreChanged { get => _onScoreChanged; set => _onScoreChanged = value; }
        public int Score 
        {
            get => _score;
            set 
            {
                _score = value;
                _hud.UpdateScoreText(_score, _saveData.Data.ClassicGameHighscore);
                OnScoreChanged?.Invoke(_score);
            }
        }
        public int Hits
        {
            get => _hits;
            set
            {
                _hits = value;
                _hud.UpdateHitsText(_hits, _saveData.Data.PreviousGameTry.Hits);
            }
        }
        public float CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                _hud.UpdateTimeText(_currentTime, _saveData.Data.PreviousGameTry.Time);
            }
        }

        private IGameSaveDataController _saveData;
        private IAudioService _audioService;
        private IPauseService _pauseService;
        private IGameHUDWithAdditionalDataView _hud;

        private CameraController _camera;
        private Action<int> _onScoreChanged;

        private bool _playTimer;
        private float _currentTime;
        private int _score;
        private int _hits;

        private int _maxStyleStreak;

        public void Initialize(CameraController camera, IGameHUD hud, Action leaveToMenuAction)
        {
            _camera = camera;
            _hud = hud as IGameHUDWithAdditionalDataView;

            _saveData = Services.Instance.Get<IGameSaveDataController>();
            _audioService = Services.Instance.Get<IAudioService>();
            _pauseService = Services.Instance.Get<IPauseService>();

            Score = 0;
            Hits = 0;
            CurrentTime = 0f;

            _hud.Initialize(leaveToMenuAction);
            Stylemeter.Initialize();

            _hud.UpdatePreviousGameDataText(_saveData.Data.PreviousGameTry.Hits, _saveData.Data.PreviousGameTry.Time);
            _hud.ClosePause();
        }

        private void Update()
        {
            if(!_pauseService.PauseEnabled)
            {
                if(_playTimer)
                    CurrentTime += Time.deltaTime;
                
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    _hud.OpenPause();
                }
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if(!Application.isEditor && !focus && !_pauseService.PauseEnabled)
                _hud.OpenPause();
        }

        public void AddScore(float style, Transform instanceBall)
        {
            ScoreData data = CalculateScoreDataFromStyle(style);

            Score += data.Score;
            Hits++;

            Stylemeter.AddStyle(data);

            // 'slow-motion'
            if(data.Style == ScoreData.MAX_STYLE)
            {
                _audioService.PlaySound(BounceSoundPerfectStart, transform);
                _pauseService.Pause();
                _camera.FocusOn(instanceBall, 5f, 0.9f, () =>
                {
                    _audioService.PlaySound(BounceSoundPerfectEnd, transform);
                    _pauseService.Resume();

                    var particle = BallParticlesController.Particles.Get();
                    particle.Play(instanceBall.position, Quaternion.identity, false);
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

        public void Restart()
        {
            var previousData = _saveData.Data.PreviousGameTry;

            if(Score > _saveData.Data.ClassicGameHighscore)
            {
                _saveData.Data.ClassicGameHighscore = Score;
            }

            previousData.Time = CurrentTime;
            previousData.Hits = Hits;

            _saveData.Save();

            _camera.Shake(
                range: new Vector4(-10, 10, -4, 4),
                speed: 1f, 
                duration: 0.7f
                );

            _hud.UpdatePreviousGameDataText(previousData.Hits, previousData.Time);

            Score = 0;
            Hits = 0;
            CurrentTime = 0;

            _maxStyleStreak = 0;

            Stylemeter.ResetStyle();
        }

        public void StopTimer()
        {
            _playTimer = false;
        }
        public void StartTimer()
        {
            _playTimer = true;
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
