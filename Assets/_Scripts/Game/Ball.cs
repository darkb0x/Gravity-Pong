using UnityEngine;
using GravityPong.Player;
using System.Collections;

namespace GravityPong.Game
{
    using Singleplayer;

    public class Ball : MonoBehaviour
    {
        private const float DEATH_ZONE_Y = -4f;
        private const float START_POS_Y = 2f;
        private const float DISABLED_SPRITE_ALPHA = 0.13f;

        [SerializeField] private Collider2D BallCollider;
        [SerializeField] private SpriteRenderer BallSprite;
        [SerializeField] private TrailRenderer Trail;

        [Header("Audio")]
        [SerializeField] private AudioClip BounceFromWallSound;
        [Space]
        [SerializeField] private AudioClip BounceSoundLow; // <176 force
        [SerializeField] private AudioClip BounceSoundMedium; // 176-200 force
        [SerializeField] private AudioClip BounceSoundEpic; // 201-300 force
        [SerializeField] private AudioClip BounceSoundUltra; // >300 force

        private IAudioService _audioService;
        private Rigidbody2D _rigidbody2D;
        private int _reboundsFromWallCount;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _audioService = Services.Instance.Get<IAudioService>();
            Restart();
        }

        private void Update()
        {
            if(transform.localPosition.y < DEATH_ZONE_Y)
            {
                Restart();
            }
        }

        private void Restart()
            => StartCoroutine(ReturnCoroutine());

        private IEnumerator ReturnCoroutine()
        {
            Vector3 startPos = new Vector3(0, START_POS_Y, 0);
            float returnSpeedLinear = 2f;
            float returnSpeedLerp = 3f;
            float minDistance = 0.05f;

            // Set ball invulnerability
            SetTransparentcyToSprite(DISABLED_SPRITE_ALPHA);
            Trail.enabled = false;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.isKinematic = true;
            BallCollider.isTrigger = true;
            _reboundsFromWallCount = 0;

            // Move ball to the start position
            while (transform.localPosition != startPos)
            {
                if (Vector3.Distance(transform.localPosition, startPos) <= minDistance)
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPos, returnSpeedLinear * Time.deltaTime);
                else
                    transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, returnSpeedLerp * Time.deltaTime);

                yield return null;
            }

            // Return ball to normal state
            SetTransparentcyToSprite(1f);
            _rigidbody2D.isKinematic = false;
            BallCollider.isTrigger = false;
            Trail.Clear();
            Trail.enabled = true;

            _rigidbody2D.velocity = new Vector2(Random.Range(-2, 2), 3f);

            // Restart game
            SingleplayerGameManager.Instance?.ResetValues();
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

                if (dirForce <= 175) // bottom
                {
                    SingleplayerGameManager.Instance.AddScore(new ScoreData(1, 0.1f, "center"));
                    PlaySound(BounceSoundLow);
                }
                else if (dirForce > 175 && dirForce <= 220) // bottom-angle
                {
                    SingleplayerGameManager.Instance.AddScore(new ScoreData(3, 0.2f, "near"));
                    PlaySound(BounceSoundMedium);
                }
                else if (dirForce > 220 && dirForce < 300) // angle
                {
                    SingleplayerGameManager.Instance.AddScore(new ScoreData(5, 0.5f, "angle"));
                    PlaySound(BounceSoundEpic);
                }
                else if(dirForce >= 300) // sides
                {
                    SingleplayerGameManager.Instance.AddScore(new ScoreData(17, 1f, "side"));
                    PlaySound(BounceSoundUltra);
                }

                _rigidbody2D.velocity = dir;

                _reboundsFromWallCount = 0;
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
    }
}