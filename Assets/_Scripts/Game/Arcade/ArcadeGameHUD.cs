using GravityPong.Pause;
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GravityPong.Game.Arcade
{
    public class ArcadeGameHUD : MonoBehaviour, IGameHUD
    {
        [SerializeField] private TMP_Text ScoreTitleText;
        [SerializeField] private TMP_Text ScoreValueText;
        [SerializeField] private GameObject NewScoreTextObj;
        [Space]
        [SerializeField] private TMP_Text StreakText;
        [SerializeField] private Animation StreakAnim;
        [SerializeField] private Vector2 StreakTextPositionXClamp;

        [Header("Pause")]
        [SerializeField] private Menu.UIButton PauseButton;
        [SerializeField] private PauseWindow PauseWindow;

        private RectTransform _streakObjRect;
        private Color32 _defaultTextColor = new Color32(255, 255, 255, 255);
        private Color32 _newHighscoreTextColor = new Color32(255, 245, 90, 255);

        public void Initialize(Action leaveToMenu)
        {
            _streakObjRect = StreakText.GetComponent<RectTransform>();

            PauseWindow.Initialize(leaveToMenu);
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

        public void ShowStreak(int value)
        {
            StreakText.text = $"Streak: {value}";
            _streakObjRect.localPosition =
                new Vector3(Random.Range(StreakTextPositionXClamp.x, StreakTextPositionXClamp.y),
                0,
                0);
            StreakAnim.Play();
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
    }
}
