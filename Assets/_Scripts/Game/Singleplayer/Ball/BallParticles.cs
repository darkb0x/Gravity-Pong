using UnityEngine;

namespace GravityPong.Game.Singleplayer.Ball
{
    public class BallParticles : MonoBehaviour
    {
        [SerializeField] private GameObject HitWallParticlePrefab;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void PlayHit(Vector3 pos)
        {
            Instantiate(HitWallParticlePrefab, pos, Quaternion.LookRotation(_rigidbody2D.velocity.normalized));
        }
    }
}