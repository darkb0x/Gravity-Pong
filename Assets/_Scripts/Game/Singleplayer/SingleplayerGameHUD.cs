using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GravityPong.Pause;
using System;

namespace GravityPong.Game.Singleplayer
{
    public class SingleplayerGameHUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text DebugText;
        [Space]
        [SerializeField] private TMP_Text ScoreText;
        [SerializeField] private TMP_Text HitsText;
        [SerializeField] private TMP_Text TimeText;
        [Space]
        [SerializeField] private Menu.UIButton LeaveButton;

        [Header("Pause")]
        [SerializeField] private Menu.UIButton PauseButton;
        [SerializeField] private PauseWindow PauseWindow;

        private Color32 _defaultTextColor = new Color32(255, 255, 255, 255);
        private Color32 _newHighscoreTextColor = new Color32(255, 245, 90, 255);

        public void Initialize(Action leaveButtonAction)
        {
            PauseWindow.Initialize();
            LeaveButton.Initialize(leaveButtonAction);
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
                ScoreText.color = _newHighscoreTextColor;
                ScoreText.text = score.ToString() + "!";
            }
            else
            {
                ScoreText.color = _defaultTextColor;
                ScoreText.text = score.ToString();
            }
        }
        public void UpdateHitsText(int value)
        {
            HitsText.text = $"Hits: {value}";
        }
        public void UpdateTimeText(float value)
        {
            TimeText.text = $"Time: {value.ToString("F1")}s";
        }
    }
}