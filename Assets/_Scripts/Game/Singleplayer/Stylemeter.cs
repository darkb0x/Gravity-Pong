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
        private const float STYLEMETER_IMAGE_COLOR_RETURN_TIME = 1.2f;

        [SerializeField] private TMP_Text StyleHistoryText;
        [SerializeField] private Gradient StylemeterColors;
        [Space]
        [SerializeField] private Image StylemeterImage;
        [Space]
        [SerializeField] private float DissapearStyleSpeed = 0.5f;
        [SerializeField] private int MaxRowsInStory = 5;

        public Action OnStyleEqualsZero;
        public float Value => _stylemeterValue;

        private List<string> _styleHistory;
        private float _stylemeterValue;

        public void Initialize(float value = 0)
        {
            _styleHistory = new();
            ResetStyle(value);
            UpdateStyleHistory();
        }
        private void Update()
        {
            _stylemeterValue = Mathf.Clamp01(_stylemeterValue -= DissapearStyleSpeed * Time.deltaTime);
            if (_stylemeterValue == 0)
                OnStyleEqualsZero?.Invoke();

            UpdateStyleTimeVisual(_stylemeterValue);

            StylemeterImage.color = Color.Lerp(StylemeterImage.color, Color.white, STYLEMETER_IMAGE_COLOR_RETURN_TIME * Time.deltaTime);
        }

        public void AddStyle(ScoreData scoreData)
        {
            _stylemeterValue = Mathf.Clamp01(_stylemeterValue + scoreData.Style);

            if (_styleHistory.Count == MaxRowsInStory)
                _styleHistory.RemoveAt(MaxRowsInStory - 1);

            Color color = StylemeterColors.Evaluate(scoreData.Style);
            _styleHistory.Insert(0, $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>+{scoreData.Score} {scoreData.StyleMessage}</color>\n");
            StylemeterImage.color = color;

            UpdateStyleHistory();
        }
        public void ResetStyle()
            => StartCoroutine(ResetStyleCoroutine());
        public void ResetStyle(float startValue)
        {
            ResetStyle();

            _stylemeterValue = startValue;
            UpdateStyleTimeVisual(_stylemeterValue);
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
        private IEnumerator ResetStyleCoroutine()
        {
            float speed = 2;

            while (StyleHistoryText.alpha > 0)
            {
                yield return null;
                float alpha = Mathf.MoveTowards(StyleHistoryText.alpha, 0, speed * Time.deltaTime);
                StyleHistoryText.alpha = alpha;
            }

            _styleHistory.Clear();
            UpdateStyleHistory();

            StyleHistoryText.alpha = 1f;
        }
    }
}
