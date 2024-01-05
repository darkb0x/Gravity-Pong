using UnityEngine;
using TMPro;
using GravityPong.Pause;
using System;
using Random = UnityEngine.Random;

namespace GravityPong.Game.Singleplayer
{
    public class SingleplayerGameHUD : MonoBehaviour, IGameHUDWithAdditionalDataView
    {
        [SerializeField] private TMP_Text ScoreTitleText;
        [SerializeField] private TMP_Text ScoreValueText;
        [SerializeField] private GameObject NewScoreTextObj;
        [Space]
        [SerializeField] private TMP_Text HitsText;
        [SerializeField] private TMP_Text TimeText;
        [SerializeField] private TMP_Text PreviousHitsText;
        [SerializeField] private TMP_Text PreviousTimeText;
        [Space]
        [SerializeField] private TMP_Text StreakText;
        [SerializeField] private Animation StreakAnim;
        [SerializeField] private RectTransform StreakObjRect;
        [SerializeField] private Vector2 StreakTextPositionXClamp;

        [Header("Pause")]
        [SerializeField] private Menu.UIButton PauseButton;
        [SerializeField] private PauseWindow PauseWindow;

        private Color32 _defaultTextColor = new Color32(255, 255, 255, 255);
        private Color32 _newHighscoreTextColor = new Color32(255, 245, 90, 255);

        public void Initialize(Action leaveButtonAction)
        {
            PauseWindow.Initialize(leaveButtonAction);
            PauseButton.Initialize(PauseWindow.Open);
        }

        public void OpenPause()
        {
            PauseWindow.Open();
        }
        public void ClosePause()
        {
            PauseWindow.Close();
        }

        public void UpdateScoreText(int score, int previousHighscore)
        {
            if (score > previousHighscore)
            {
                ScoreValueText.color = _newHighscoreTextColor;
                ScoreTitleText.color = _newHighscoreTextColor;
                ScoreValueText.text = score.ToString() + "!";
                NewScoreTextObj.SetActive(true);
            }
            else
            {
                ScoreValueText.color = _defaultTextColor;
                ScoreTitleText.color = _defaultTextColor;
                ScoreValueText.text = score.ToString();
                NewScoreTextObj.SetActive(false);
            }
        }
        public void UpdateHitsText(int value, int previous)
        {
            if(value > previous)
                HitsText.text = $"Hits: <color=#{ColorUtility.ToHtmlStringRGB(_newHighscoreTextColor)}>{value}";
            else
                HitsText.text = $"Hits: <color=#{ColorUtility.ToHtmlStringRGB(_defaultTextColor)}>{value}";
        }
        public void UpdateTimeText(float value, float previous)
        {
            if (value > previous)
                TimeText.text = $"Time: <color=#{ColorUtility.ToHtmlStringRGB(_newHighscoreTextColor)}>{value.ToString("F1")}s";
            else
                TimeText.text = $"Time: <color=#{ColorUtility.ToHtmlStringRGB(_defaultTextColor)}>{value.ToString("F1")}s";
        }
        public void UpdatePreviousGameDataText(int hits, float time)
        {
            PreviousHitsText.text = $"Hits: {hits}";
            PreviousTimeText.text = $"Time: {time.ToString("F1")}s";
        }

        public void ShowStreak(int value)
        {
            StreakText.text = $"Streak: {value}";
            StreakObjRect.localPosition =
                new Vector3(Random.Range(StreakTextPositionXClamp.x, StreakTextPositionXClamp.y),
                0,
                0);
            StreakAnim.Play();
        }
    }
}