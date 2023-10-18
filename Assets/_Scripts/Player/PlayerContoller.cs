using UnityEngine;

namespace GravityPong.Player
{
    public class PlayerContoller : MonoBehaviour
    {
        [SerializeField] private float Speed = 2f;

        private IInputService _input;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _direction;
        private float _changeDirSpeed;

        private void Awake()
        {
            _input = Services.Instance.Get<IInputService>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _direction = Vector2.zero;
            _changeDirSpeed = .3f;
        }

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
            _rigidbody2D.velocity = _direction * Speed * Time.fixedDeltaTime;
        }
    }
}
