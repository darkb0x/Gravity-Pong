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
                _hud.UpdateScoreText(_score, _previousHighscore);
                OnScoreChanged?.Invoke(_score);
            }
        }
        public int Hits
        {
            get => _hits;
            set
            {
                _hits = value;
                _previousRoundHits = value;
                _hud.UpdateHitsText(_hits, _previousHits);
            }
        }
        public float CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                _previoudRoundTime = value;
                _hud.UpdateTimeText(_currentTime, _previousTime);
            }
        }

        private IAudioService _audioService;
        private IPauseService _pauseService;
        private IGameHUDWithAdditionalDataView _hud;

        private CameraController _camera;
        private Action<int> _onScoreChanged;

        private bool _playTimer;
        private float _currentTime;
        private float _previousTime;
        private int _score;
        private int _hits;

        private int _previousHighscore;
        private int _previousHits;
        private int _maxStyleStreak;

        private int _previousRoundHits;
        private float _previoudRoundTime;

        public void Initialize(CameraController camera, IGameHUD hud, Action leaveToMenuAction)
        {
            _camera = camera;
            _hud = hud as IGameHUDWithAdditionalDataView;

            _audioService = Services.Instance.Get<IAudioService>();
            _pauseService = Services.Instance.Get<IPauseService>();

            _previousHighscore = PlayerPrefs.GetInt(Constants.PlayerPrefs.HIGHSCORE_KEY);
            _previousHits = PlayerPrefs.GetInt(Constants.PlayerPrefs.RECORD_OF_HITS_KEY);
            _previousTime = PlayerPrefs.GetFloat(Constants.PlayerPrefs.RECORD_OF_TIME_KEY);

            Score = 0;
            Hits = 0;
            CurrentTime = 0f;

            _hud.Initialize(leaveToMenuAction);
            Stylemeter.Initialize();

            _hud.UpdatePreviousGameDataText(_previousRoundHits, _previoudRoundTime);
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
            if(Score > _previousHighscore)
            {
                PlayerPrefs.SetInt(Constants.PlayerPrefs.HIGHSCORE_KEY, Score);
                _previousHighscore = PlayerPrefs.GetInt(Constants.PlayerPrefs.HIGHSCORE_KEY);
            }
            if(CurrentTime > _previousTime)
            {
                PlayerPrefs.SetFloat(Constants.PlayerPrefs.RECORD_OF_TIME_KEY, CurrentTime);
                _previousTime = PlayerPrefs.GetFloat(Constants.PlayerPrefs.RECORD_OF_TIME_KEY);
            }
            if(Hits > _previousHits)
            {
                PlayerPrefs.SetInt(Constants.PlayerPrefs.RECORD_OF_HITS_KEY, Hits);
                _previousHits = PlayerPrefs.GetInt(Constants.PlayerPrefs.RECORD_OF_HITS_KEY);
            }
            PlayerPrefs.Save();

            _camera.Shake(new Vector4(-10, 10, -4, 4), 1f, 0.7f);

            _hud.UpdatePreviousGameDataText(_previousRoundHits, _previoudRoundTime);

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
