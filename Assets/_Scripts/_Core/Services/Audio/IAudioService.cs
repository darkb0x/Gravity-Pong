using UnityEngine;

namespace GravityPong
{
    public interface IAudioService : IService
    {
        AudioSource PlaySound(AudioClip clip, Transform instance);
    }
}