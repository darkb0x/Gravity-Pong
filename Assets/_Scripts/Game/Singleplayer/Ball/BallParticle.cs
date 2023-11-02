using GravityPong.Utilities;
using UnityEngine;

namespace GravityPong.Game.Singleplayer.Ball
{
    public class BallParticle : MonoBehaviour
    {
        [SerializeField] private float HideTime = 2f;
        [Space]
        [SerializeField] private ParticleSystem MainParticles;
        [SerializeField] private ParticleSystem SubParticles1;
        [SerializeField] private ParticleSystem SubParticles2;
        [Space]
        [SerializeField] private Animation Anim;

        private ObjectPool<BallParticle> _objectPool => BallParticlesController.Particles;

        public void Play(Vector3 pos, Quaternion rotation, bool onlyParticles)
        {
            transform.position = pos;
            transform.rotation = rotation;

            PlayAllEffects();
            if(!onlyParticles)
                Anim.Play();

            Invoke(nameof(ReturnToPool), HideTime);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void PlayAllEffects()
        {
            MainParticles.Play();
            SubParticles1.Play();
            SubParticles2.Play();
        }
        private void ReturnToPool()
        {
            _objectPool.Return(this);
        }
    }
}