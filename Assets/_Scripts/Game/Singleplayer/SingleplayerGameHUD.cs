using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GravityPong.Pause;
using System;
using UnityEngine.SocialPlatforms.Impl;

namespace GravityPong.Game.Singleplayer
{
    public class SingleplayerGameHUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text DebugText;
        [Space]
        [SerializeField] private TMP_Text ScoreTitleText;
        [SerializeField] private TMP_Text ScoreValueText;
        [SerializeField] private GameObject NewScoreTextObj;
        [Space]
        [SerializeField] private TMP_Text HitsText;
        [SerializeField] private TMP_Text TimeText;
        [SerializeField] private TMP_Text PreviousHitsText;
        [SerializeField] private TMP_Text PreviousTimeText;

        [Header("Pause")]
        [SerializeField] private Menu.UIButton PauseButton;
        [SerializeField] private PauseWindow PauseWindow;

        private Color32 _defaultTextColor = new Color32(255, 255, 255, 255);
        private Color32 _newHighscoreTextColor = new Color32(255, 245, 90, 255);

        public void Initialize(Action leaveButtonAction)
        {
            PauseWindow.Initialize(leaveButtonAction);
            PauseButton.Initialize(PauseWindow.Open);

            if (!Application.isEditor)
                DebugText.gameObject.SetActive(false);
        }

        public void OpenPause()
        {
            PauseWindow.Open();
        }
        public void ClosePause()
        {
            PauseWindow.Close();
        }

        public void SetDebugText(string text)
        {
            DebugText.text = text;
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
    }
}