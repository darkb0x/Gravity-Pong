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
            Vector3 targetPos = new Vector3(0, START_POS_Y, 0);
            float returnSped = 2f;

            SetTransparentcyToSprite(DISABLED_SPRITE_ALPHA);
            Trail.enabled = false;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.isKinematic = true;
            BallCollider.isTrigger = true;
            _reboundsFromWallCount = 0;

            while (transform.localPosition != targetPos)
            {
                yield return null;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, returnSped * Time.deltaTime);
            }

            SetTransparentcyToSprite(1f);
            _rigidbody2D.isKinematic = false;
            BallCollider.isTrigger = false;
            Trail.Clear();
            Trail.enabled = true;

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
                    SingleplayerGameManager.Instance.AddScore(1);
                    PlaySound(BounceSoundLow);
                }
                else if (dirForce > 175 && dirForce <= 200) // bottom-angle
                {
                    SingleplayerGameManager.Instance.AddScore(3);
                    PlaySound(BounceSoundMedium);
                }
                else if (dirForce > 200 && dirForce < 300) // angle
                {
                    SingleplayerGameManager.Instance.AddScore(5);
                    PlaySound(BounceSoundEpic);
                }
                else if(dirForce >= 300) // sides
                {
                    SingleplayerGameManager.Instance.AddScore(20);
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