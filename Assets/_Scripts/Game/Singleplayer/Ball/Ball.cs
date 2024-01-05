using UnityEngine;
using System.Collections;
using GravityPong.Pause;
using GravityPong.Game.Singleplayer.Player;

namespace GravityPong.Game.Singleplayer.Ball
{
    public class Ball : MonoBehaviour
    {
        private const float DEATH_ZONE_Y = -4f;
        private const float START_POS_Y = 2f;
        private const float DISABLED_SPRITE_ALPHA = 0.13f;

        [SerializeField] private Collider2D BallCollider;
        [SerializeField] private SpriteRenderer BallSprite;

        [Header("Audio")]
        [SerializeField] private AudioClip BounceFromWallSound;
        [Space]
        [SerializeField] private AudioClip BounceSoundLow; // <176 force
        [SerializeField] private AudioClip BounceSoundMedium; // 176-200 force
        [SerializeField] private AudioClip BounceSoundEpic; // 201-300 force

        [Header("Particles")]
        [SerializeField] private BallParticlesController BallParticles;

        private IGameModeManager _gameManager;
        private Rigidbody2D _rigidbody2D;
        
        private IAudioService _audioService;
        private IPauseService _pauseService;

        private Vector2 _savedVelocity;
        private RigidbodyType2D _savedRigidBody2DType;

        private bool _isReturning;
        private int _reboundsFromWallCount;

        public void Initialize(IGameModeManager gameManager)
        {
            _gameManager = gameManager;
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _audioService = Services.Instance.Get<IAudioService>();
            _pauseService = Services.Instance.Get<IPauseService>();

            SubscribeToEvents();

            Restart(false);
        }
        private void OnDestroy()
            => UnsubscribeFromEvents();

        private void Update()
        {
            if (transform.localPosition.y < DEATH_ZONE_Y)
            {
                Restart();
            }
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {  
            if(coll.gameObject.TryGetComponent(out PlayerContoller _))
            {
                BounceFromPlayer(coll);
            }
            else
            {
                BounceFromWall();
            }
        }

        #region Restart Function
        private void Restart(bool isGameLoop = true)
            => StartCoroutine(ReturnCoroutine(isGameLoop));

        private IEnumerator ReturnCoroutine(bool isGameLoop)
        {
            Vector3 startPos = new Vector3(0, START_POS_Y, 0);
            float returnSpeedLinear = 2f;
            float returnSpeedLerp = 3f;
            float minDistance = 0.05f;

            _gameManager.StopTimer();

            // Restart game
            if (isGameLoop)
            {
                _gameManager.Restart();
            }

            // Set ball invulnerability
            _isReturning = true;
            DisableBall();

            // Move ball to the start position
            while (transform.localPosition != startPos && _isReturning)
            {
                if (_pauseService.PauseEnabled)
                    yield return null;

                if (Vector3.Distance(transform.localPosition, startPos) <= minDistance)
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, returnSpeedLinear * Time.deltaTime);
                else
                    transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, returnSpeedLerp * Time.deltaTime);

                yield return null;
            }

            // Return ball to normal state
            _isReturning = false;

            EnableBall();

            int randomValue = Random.Range(0, 2);
            if (randomValue == 0)
            {
                _rigidbody2D.velocity = new Vector2(-1.5f, 3f);
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(1.5f, 3f);
            }

            _gameManager.StartTimer();
        }

        private void EnableBall()
        {
            SetTransparentcyToSprite(1f);
            _rigidbody2D.isKinematic = false;
            BallCollider.isTrigger = false;
        }
        private void DisableBall()
        {

            SetTransparentcyToSprite(DISABLED_SPRITE_ALPHA);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.isKinematic = true;
            BallCollider.isTrigger = true;
            _reboundsFromWallCount = 0;
        }
        private void SetTransparentcyToSprite(float alpha)
        {
            BallSprite.color = new Color(1, 1, 1, alpha);
        }
        #endregion

        #region Bounces
        private void BounceFromPlayer(Collision2D playerCollision)
        {
            float x = (transform.position - playerCollision.transform.position).x;

            float forceY = 8.5f;
            float forceXFactor = 10f;
            float gravity = 1.5f;
            Vector2 dir = new Vector2(x * forceXFactor, forceY) * gravity;
            float dirForce = dir.sqrMagnitude;

            if (_rigidbody2D.bodyType != RigidbodyType2D.Static)
                _rigidbody2D.velocity = dir;

            if (dirForce <= 175) // bottom
            {
                AddScore(0.25f);
                PlaySound(BounceSoundLow);
            }
            else if (dirForce > 175 && dirForce <= 220) // bottom-angle
            {
                AddScore(0.5f);
                PlaySound(BounceSoundMedium);
            }
            else if (dirForce > 220 && dirForce < 300) // angle
            {
                AddScore(0.75f);
                PlaySound(BounceSoundEpic);
            }
            else if (dirForce >= 300) // sides
            {
                AddScore(1f);
            }

            _reboundsFromWallCount = 0;
        }

        private void BounceFromWall()
        {
            _reboundsFromWallCount++;

            if (_reboundsFromWallCount >= 2)
            {
                float velocityDivider = 1.6f;
                _rigidbody2D.velocity /= velocityDivider;
            }

            PlaySound(BounceFromWallSound);
        }

        private void AddScore(float style)
            => _gameManager.AddScore(style, transform);
        #endregion

        #region Events
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
            if(pause)
            {
                _savedVelocity = _rigidbody2D.velocity;
                _savedRigidBody2DType = _rigidbody2D.bodyType;

                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
            else
            {
                _rigidbody2D.bodyType = _savedRigidBody2DType;
                _rigidbody2D.velocity = _savedVelocity;
            }
        }
        #endregion

        private void PlaySound(AudioClip clip)
        {
            _audioService.PlaySound(clip, transform);
        }
    }
}