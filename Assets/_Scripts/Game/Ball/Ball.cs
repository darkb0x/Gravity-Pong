using UnityEngine;
using GravityPong.Player;
using System.Collections;

namespace GravityPong.Game
{
    using GravityPong.Pause;
    using Singleplayer;
    using Unity.VisualScripting;

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
        [SerializeField] private BallParticles BallParticles;

        private Rigidbody2D _rigidbody2D;
        
        private IAudioService _audioService;
        private IPauseService _pauseService;

        private Vector2 _savedVelocity;
        private RigidbodyType2D _savedRigidBody2DType;

        private bool _isReturning;
        private int _reboundsFromWallCount;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _audioService = Services.Instance.Get<IAudioService>();
            _pauseService = Services.Instance.Get<IPauseService>();

            SubscribeToEvents();
        }
        private void Start()
        {
            Restart(false);
        }
        private void OnDestroy()
            => UnsubscribeFromEvents();

        private void Update()
        {
            if(transform.localPosition.y < DEATH_ZONE_Y)
            {
                Restart();
            }
        }

        private void Restart(bool isGameLoop = true)
            => StartCoroutine(ReturnCoroutine(isGameLoop));

        private IEnumerator ReturnCoroutine(bool isGameLoop)
        {
            Vector3 startPos = new Vector3(0, START_POS_Y, 0);
            float returnSpeedLinear = 2f;
            float returnSpeedLerp = 3f;
            float minDistance = 0.05f;

            SingleplayerGameManager.Instance?.StopTimer();
            
            // Restart game
            if(isGameLoop)
            {
                SingleplayerGameManager.Instance?.Restart();
            }

            // Set ball invulnerability
            _isReturning = true;

            SetTransparentcyToSprite(DISABLED_SPRITE_ALPHA);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.isKinematic = true;
            BallCollider.isTrigger = true;
            _reboundsFromWallCount = 0;

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

            SetTransparentcyToSprite(1f);
            _rigidbody2D.isKinematic = false;
            BallCollider.isTrigger = false;

            int randomValue = Random.Range(0, 2);
            if(randomValue == 0)
            {
                _rigidbody2D.velocity = new Vector2(-1.5f, 3f);
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(1.5f, 3f);
            }

            SingleplayerGameManager.Instance?.StartTimer();        
        }

        private void SetTransparentcyToSprite(float alpha)
        {
            BallSprite.color = new Color(1, 1, 1, alpha);
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {  
            if(coll.gameObject.TryGetComponent(out PlayerContoller _))
            {
                float x = (transform.position - coll.transform.position).x;

                float forceY = 8.5f;
                float forceXFactor = 10f;
                float gravity = 1.5f;
                Vector2 dir = new Vector2(x * forceXFactor, forceY) * gravity;
                float dirForce = dir.sqrMagnitude;
                SingleplayerGameManager.Instance.SetDebugText("Force: " + dirForce);

                if(_rigidbody2D.bodyType != RigidbodyType2D.Static)
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
                else if(dirForce >= 300) // sides
                {
                    AddScore(1f);
                }

                _reboundsFromWallCount = 0;
                BallParticles.PlayHit(new Vector3(transform.position.x, coll.transform.position.y));
            }
            else
            {
                _reboundsFromWallCount++;

                if(_reboundsFromWallCount >= 2)
                {
                    float velocityDivider = 2f;
                    _rigidbody2D.velocity /= velocityDivider;
                }

                PlaySound(BounceFromWallSound);
            }
        }

        private void PlaySound(AudioClip clip)
        {
            _audioService.PlaySound(clip, transform);
        }
        private void AddScore(float style)
        {
            int score;
            string styleMessage;

            switch (style)
            {
                case 0.25f:
                    score = 1;
                    styleMessage = "center";
                    break;
                case 0.5f:
                    score = 3;
                    styleMessage = "side";
                    break;
                case 0.75f:
                    score = 5;
                    styleMessage = "edge";
                    break;
                case 1f:
                    score = 20;
                    styleMessage = "vertical";
                    break;
                default:
                    goto case 0.25f;
            }

            SingleplayerGameManager.Instance.AddScore(new ScoreData(score, style, styleMessage), transform);
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
    }
}