using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GravityPong.Game
{
    public class ScreenColorController : MonoBehaviour
    {
        [System.Serializable]
        struct ScreenColorData
        {
            public int RequiredScore;
            public Color Color;
        }

        [SerializeField] private ScreenColorData[] _screenColors;

        private Camera _cam;
        private Coroutine _currentChangeColorCoroutine;
        private IGameModeManager _gameModeManager;
        private int _currentScreenColorIndex;

        public void Initialize(IGameModeManager gameModeManager, Camera cam)
        {
            _gameModeManager = gameModeManager;
            _cam = cam;
            _currentScreenColorIndex = 0;

            _gameModeManager.OnScoreChanged += OnScoreUpdated;

            OnScoreUpdated(0);
        }

        private void OnDestroy()
        {
            if (_gameModeManager == null)
                return;

            _gameModeManager.OnScoreChanged -= OnScoreUpdated;
        }

        private void OnScoreUpdated(int value)
        {
            bool changeBG = false;

            if (_currentScreenColorIndex >= _screenColors.Length && value >= _screenColors[_currentScreenColorIndex].RequiredScore)
                return;
            if (_screenColors[_currentScreenColorIndex + 1].RequiredScore <= value)
            {
                _currentScreenColorIndex++;
                changeBG = true;
            }
            else if(_screenColors[_currentScreenColorIndex].RequiredScore > value)
            {
                do
                {
                    _currentScreenColorIndex--;
                } while (_screenColors[_currentScreenColorIndex].RequiredScore > value);
                changeBG = true;
            }

            if(changeBG)
            {
                if (_currentChangeColorCoroutine != null)
                    StopCoroutine(_currentChangeColorCoroutine);
                _currentChangeColorCoroutine = StartCoroutine(ChangeColorCoroutine(_screenColors[_currentScreenColorIndex].Color));
            }
        }

        private IEnumerator ChangeColorCoroutine(Color target)
        {
            Color init = _cam.backgroundColor;
            float speed = 0.02f;
            float
                r = init.r,
                g = init.g,
                b = init.b;

            while (_cam.backgroundColor != target)
            {
                Color current = new Color(r, g, b, 1f);
                r = Mathf.MoveTowards(r, target.r, speed * Time.deltaTime);
                g = Mathf.MoveTowards(g, target.g, speed * Time.deltaTime);
                b = Mathf.MoveTowards(b, target.b, speed * Time.deltaTime);

                _cam.backgroundColor = current;

                yield return null;
            }

            _currentChangeColorCoroutine = null;
        }
    }
}
