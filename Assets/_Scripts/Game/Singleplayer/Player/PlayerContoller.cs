using GravityPong.Pause;
using UnityEngine;

namespace GravityPong.Game.Singleplayer.Player
{
    public class PlayerContoller : MonoBehaviour
    {
        [SerializeField] private float Speed = 2f;

        private Rigidbody2D _rigidbody2D;

        private IInputService _input;
        private IPauseService _pauseService;

        private Vector2 _direction;
        private bool _paused;
        private float _changeDirSpeed;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _input = Services.Instance.Get<IInputService>();
            _pauseService = Services.Instance.Get<IPauseService>();

            _direction = Vector2.zero;
            _changeDirSpeed = .3f;

            SubscribeToEvents();
        }
        private void OnDestroy()
            => UnsubscribeFromEvents();

        private void Update()
        {
            float horizontal = _input.GetHorizontal();
            float factor = 0.5f;

            if(horizontal == 0)
            {
                factor = 2f;
            }

            _direction = Vector2.Lerp(_direction, new Vector2(horizontal, 0), _changeDirSpeed * factor);
        }

        private void FixedUpdate()
        {
            if(!_paused)
                _rigidbody2D.velocity = _direction * Speed * Time.fixedDeltaTime;
        }

        private void SubscribeToEvents()
        {
            _pauseService.OnPauseStateChanged += OnPauseStateChanged;

            OnPauseStateChanged(_pauseService.PauseEnabled);
        }
        private void UnsubscribeFromEvents()
        {
            _pauseService.OnPauseStateChanged -= OnPauseStateChanged;
        }
        private void OnPauseStateChanged(bool pause)
        {
            _paused = pause;

            if (pause)
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }
    }
}
