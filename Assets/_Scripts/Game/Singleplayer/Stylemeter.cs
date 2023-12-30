using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GravityPong.Game.Singleplayer
{
    public class Stylemeter : MonoBehaviour
    {
        private const float SPEED = 0.5f;
        private const float STYLEMETER_IMAGE_COLOR_RETURN_TIME = 1.2f;

        [SerializeField] private TMP_Text StyleHistoryText;
        [SerializeField] private Gradient StylemeterColors;
        [Space]
        [SerializeField] private Image StylemeterImage;

        private List<string> _styleHistory;
        private float _stylemeterValue;

        public void Initialize()
        {
            _styleHistory = new();
            _stylemeterValue = 0f;

            UpdateStyleTimeVisual(_stylemeterValue);
            UpdateStyleHistory();
        }
        private void Update()
        {
            _stylemeterValue = Mathf.Clamp01(_stylemeterValue -= SPEED * Time.deltaTime);
            UpdateStyleTimeVisual(_stylemeterValue);

            StylemeterImage.color = Color.Lerp(StylemeterImage.color, Color.white, STYLEMETER_IMAGE_COLOR_RETURN_TIME * Time.deltaTime);
        }

        public void AddStyle(ScoreData scoreData)
        {
            _stylemeterValue = Mathf.Clamp01(_stylemeterValue + scoreData.Style);

            if (_styleHistory.Count == 5)
                _styleHistory.RemoveAt(4);

            Color color = StylemeterColors.Evaluate(scoreData.Style);
            _styleHistory.Insert(0, $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>+{scoreData.Score} {scoreData.StyleMessage}</color>\n");
            StylemeterImage.color = color;

            UpdateStyleHistory();
        }

        private void UpdateStyleTimeVisual(float value)
        {
            StylemeterImage.fillAmount = value / 1f;
        }
        private void UpdateStyleHistory()
        {
            string result = string.Empty;
            foreach (var item in _styleHistory)
            {
                if (!string.IsNullOrEmpty(item))
                    result += item;
            }
            StyleHistoryText.text = result;
        }
    }
}
