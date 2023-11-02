using GravityPong.Utilities;
using UnityEngine;

namespace GravityPong.Game.Singleplayer.Ball
{
    public class BallParticlesController : MonoBehaviour
    {
        private const int PARTICLES_OBJECT_POOL_AMOUNT = 10;

        public static ObjectPool<BallParticle> Particles;

        [SerializeField] private GameObject HitWallParticlePrefab;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            if(Particles == null)
                Particles = new ObjectPool<BallParticle>(SpawnBallParticle, p => p.Show(), p => p.Hide(), PARTICLES_OBJECT_POOL_AMOUNT);
        }

        public void PlayHit(Vector3 pos, bool isSideHit)
        {
            var particle = Particles.Get();
            particle.Play(pos, Quaternion.LookRotation(_rigidbody2D.velocity.normalized), !isSideHit);
        }

        private BallParticle SpawnBallParticle()
        {
            var particle = Instantiate(HitWallParticlePrefab, Vector2.zero, Quaternion.identity)
                .GetComponent<BallParticle>();

            DontDestroyOnLoad(particle);

            return particle;
        }
    }
}