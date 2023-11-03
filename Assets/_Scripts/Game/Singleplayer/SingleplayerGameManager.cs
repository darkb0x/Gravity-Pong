using UnityEngine;
using TMPro;
using GravityPong.Pause;
using GravityPong.Game.Singleplayer.Ball;

namespace GravityPong.Game.Singleplayer
{
    public class SingleplayerGameManager : MonoBehaviour
    {
        public static SingleplayerGameManager Instance;

        [Header("HUD")]
        [SerializeField] private SingleplayerGameHUD HUD;
        [SerializeField] private Stylemeter Stylemeter;

        [Header("Audio")]
        [SerializeField] private AudioClip BounceSoundPerfectStart;
        [SerializeField] private AudioClip BounceSoundPerfectEnd;

        public int Score 
        {
            get => _score;
            set 
            {
                _score = value;
                HUD.UpdateScoreText(_score, _previousHighscore);
            }
        }
        public int Hits
        {
            get => _hits;
            set
            {
                _hits = value;
                HUD.UpdateHitsText(_hits);
            }
        }
        public float CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                HUD.UpdateTimeText(_currentTime);
            }
        }

        private IAudioService _audioService;
        private IPauseService _pauseService;

        private CameraController _camera;

        private bool _playTimer;
        private float _currentTime;
        private int _previousHighscore;
        private int _score;
        private int _hits;


        private void Awake()
        {
            Instance = this;

            _audioService = Services.Instance.Get<IAudioService>();
            _pauseService = Services.Instance.Get<IPauseService>();

            _previousHighscore = PlayerPrefs.GetInt(Constants.PlayerPrefs.HIGHSCORE_PLAYERPREFS_KEY);

            HUD.Initialize(LeaveToMenu);
            HUD.SetDebugText("...");

            CurrentTime = 0f;
        }
        private void Start()
        {
            _camera = FindObjectOfType<CameraController>();

            HUD.ClosePause();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if(!_pauseService.PauseEnabled)
            {
                if(_playTimer)
                    CurrentTime += Time.deltaTime;
                
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    HUD.OpenPause();
                }
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if(!Application.isEditor && !focus && !_pauseService.PauseEnabled)
                HUD.OpenPause();
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
            }
        }

        public void Restart()
        {
            if(Score > _previousHighscore)
            {
                PlayerPrefs.SetInt(Constants.PlayerPrefs.HIGHSCORE_PLAYERPREFS_KEY, Score);
                _previousHighscore = PlayerPrefs.GetInt(Constants.PlayerPrefs.HIGHSCORE_PLAYERPREFS_KEY);
            }

            _camera.Shake(new Vector4(-10, 10, -4, 4), 1f, 0.7f);

            Score = 0;
            Hits = 0;
        }

        public void StopTimer()
        {
            _playTimer = false;
        }
        public void StartTimer()
        {
            _playTimer = true;
            CurrentTime = 0;
        }

        private ScoreData CalculateScoreDataFromStyle(float style)
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
        private void LeaveToMenu()
        {
            Services.Instance.Get<ISceneLoader>().LoadScene(Constants.Scenes.MAIN_MENU_SCENE_NAME);
        }
    }
}
