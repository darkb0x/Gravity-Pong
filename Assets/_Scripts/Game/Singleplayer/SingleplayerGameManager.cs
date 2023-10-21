using UnityEngine;
using TMPro;
using GravityPong.Pause;

namespace GravityPong.Game.Singleplayer
{
    public class SingleplayerGameManager : MonoBehaviour
    {
        public static SingleplayerGameManager Instance;

        [SerializeField] private TMP_Text DebugText;
        [Space]
        [SerializeField] private TMP_Text ScoreText;
        [SerializeField] private TMP_Text HitsText;
        [SerializeField] private TMP_Text TimeText;
        [Space]
        [SerializeField] private Menu.UIButton LeaveButton;
        [Space]
        [SerializeField] private Menu.UIButton PauseButton;
        [SerializeField] private PauseWindow PauseWindow;

        [Header("Style")]
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
                UpdateScoreText(_score);
            }
        }
        public int Hits
        {
            get => _hits;
            set
            {
                _hits = value;
                UpdateHitsText(_hits);
            }
        }
        public float CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                UpdateTimeText(_currentTime);
            }
        }

        private IAudioService _audioService;
        private IPauseService _pauseService;

        private CameraController _camera;

        private Color32 _defaultTextColor = new Color32(255, 255, 255, 255);
        private Color32 _newHighscoreTextColor = new Color32(255, 245, 90, 255);
        private float _currentTime;
        private int _previousHighscore;
        private int _score;
        private int _hits;


        private void Awake()
        {
            Instance = this;

            _audioService = Services.Instance.Get<IAudioService>();
            _pauseService = Services.Instance.Get<IPauseService>();

            _previousHighscore = PlayerPrefs.GetInt(Constants.HIGHSCORE_PLAYERPREFS_KEY);

            PauseWindow.Initialize();
            LeaveButton.Initialize(LeaveToMenu);
            PauseButton.Initialize(PauseWindow.Open);

            SetDebugText("...");
            ResetValues();

            if(!Application.isEditor)
                DebugText.gameObject.SetActive(false);
        }

        private void Start()
        {
            _camera = FindObjectOfType<CameraController>();

            PauseWindow.Close();
        }
        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime;

            if(!_pauseService.PauseEnabled)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseWindow.Open();
                }
            }
        }
        public void AddScore(ScoreData data, Transform instanceBall)
        {
            Score += data.Score;
            Hits++;

            Stylemeter.AddStyle(data);

            if(data.Style == ScoreData.MAX_STYLE)
            {
                _audioService.PlaySound(BounceSoundPerfectStart, transform);
                _pauseService.Pause();
                _camera.FocusOn(instanceBall, 5f, 0.9f, () =>
                {
                    _audioService.PlaySound(BounceSoundPerfectEnd, transform);
                    _pauseService.Resume();
                });
            }
        }
        public void Restart()
        {
            if(Score > _previousHighscore)
            {
                PlayerPrefs.SetInt(Constants.HIGHSCORE_PLAYERPREFS_KEY, Score);
                _previousHighscore = PlayerPrefs.GetInt(Constants.HIGHSCORE_PLAYERPREFS_KEY);
            }

            _camera.Shake(new Vector4(-10, 10, -4, 4), 1f, 0.7f);

            ResetValues();
        }
        private void ResetValues()
        { 
            Score = 0;
            Hits = 0;
            CurrentTime = 0;
        }


        public void SetDebugText(string text)
        {
            DebugText.text = text;
        }

        private void UpdateScoreText(int score)
        {
            if(Score > _previousHighscore)
            {
                ScoreText.color = _newHighscoreTextColor;
                ScoreText.text = score.ToString() + "!";
            }
            else
            {
                ScoreText.color = _defaultTextColor;
                ScoreText.text = score.ToString();
            }
        }
        private void UpdateHitsText(int value)
        {
            HitsText.text = $"Hits: {value}";
        }
        private void UpdateTimeText(float value)
        {
            TimeText.text = $"Time: {value.ToString("F1")}s";
        }

        private void LeaveToMenu()
        {
            Services.Instance.Get<ISceneLoader>().LoadScene(Constants.MAIN_MENU_SCENE_NAME);
        }
    }
}
