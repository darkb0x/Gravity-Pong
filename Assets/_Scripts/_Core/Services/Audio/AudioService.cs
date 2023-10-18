using UnityEngine;
using GravityPong.Utilities;
using System.Collections;

namespace GravityPong
{
    public class AudioService : IAudioService
    {
        private const string SOUND_GAMEOBJECT_PREFAB_PATH = "Prefabs/Audio";
        private const float DESTROY_AUDIO_GAMEOBJECT_DELAY = 0.5f;
        private const int AUDIO_OBJECTS_IN_OBJ_POOL = 10;

        public static GameObject SoundGOPrefab;

        private readonly Transform _parentForAudioObj;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ObjectPool<AudioSource> _audioObjectPool;

        public AudioService(GameObject audioObjParent, ICoroutineRunner coroutineRunner)
        {
            if (SoundGOPrefab == null)
                SoundGOPrefab = Resources.Load<GameObject>(SOUND_GAMEOBJECT_PREFAB_PATH);

            _parentForAudioObj = audioObjParent.transform;
            _coroutineRunner = coroutineRunner; 
            _audioObjectPool = new ObjectPool<AudioSource>(SpawnAudioObj, GetAudioObj, ReturnAudioObj, AUDIO_OBJECTS_IN_OBJ_POOL);
        }

        public AudioSource PlaySound(AudioClip clip, Transform instance)
        {
            AudioSource audioSource = _audioObjectPool.Get();
            audioSource.gameObject.name = $"Audio of: {instance.gameObject.name} (clip: {clip.name})";
            audioSource.clip = clip;
            audioSource.Play();
            _coroutineRunner.RunCoroutine(ReturnAudioObjCoroutine(audioSource));

            return audioSource;
        }

        private IEnumerator ReturnAudioObjCoroutine(AudioSource audio)
        {
            yield return new WaitForSeconds(audio.clip.length + DESTROY_AUDIO_GAMEOBJECT_DELAY);
            _audioObjectPool.Return(audio);
        }

        private AudioSource SpawnAudioObj()
        {
            return Object.Instantiate(SoundGOPrefab, _parentForAudioObj).GetComponent<AudioSource>();
        }
        private void GetAudioObj(AudioSource obj)
        {
            obj.gameObject.SetActive(true);
        }
        private void ReturnAudioObj(AudioSource obj)
        {
            obj.Stop();
            obj.gameObject.SetActive(false);
            obj.gameObject.name = "Audio of: --- (clip: ---)";
        }
    }
}