using UnityEngine;
using TMPro;

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

        [Header("Style")]
        [SerializeField] private Stylemeter Stylemeter;

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

        private Color32 _defaultTextColor = new Color32(255, 255, 255, 255);
        private Color32 _newHighscoreTextColor = new Color32(255, 245, 90, 255);
        private float _currentTime;
        private int _previousHighscore;
        private int _score;
        private int _hits;


        private void Awake()
        {
            Instance = this;

            _previousHighscore = PlayerPrefs.GetInt(Constants.HIGHSCORE_PLAYERPREFS_KEY);

            LeaveButton.Initialize(LeaveToMenu);

            SetDebugText("...");

            if(!Application.isEditor)
                DebugText.gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime;
        }
        public void AddScore(ScoreData data)
        {
            Score += data.Score;
            Hits++;

            Stylemeter.AddStyle(data.Style, data.StyleMessage);
        }
        public void ResetValues()
        {
            if(Score > _previousHighscore)
            {
                PlayerPrefs.SetInt(Constants.HIGHSCORE_PLAYERPREFS_KEY, Score);
                _previousHighscore = PlayerPrefs.GetInt(Constants.HIGHSCORE_PLAYERPREFS_KEY);
            }

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
            HitsText.text = $"Hist: {value}";
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
