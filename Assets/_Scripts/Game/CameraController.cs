using System;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using Random = UnityEngine.Random;
using GravityPong.Menu.Settings;

namespace GravityPong.Game
{
    public class CameraController : MonoBehaviour
    {
        private const float INITIAL_CAMERA_VIEW_SIZE = 5f;

        [SerializeField] private PostProcessLayer PostProcessLayer;
        [SerializeField] private PostProcessVolume StopTimePostProcessVolume;

        private Camera _camera;
        private ISettingsData _settingsData;

        private Coroutine _focusOnCoroutine;
        private Coroutine _shakeCoroutine;

        private Vector3 _initialPosition;
        private bool _isPlayingAnimation;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _settingsData = Services.Instance.Get<ISettingsData>();

            _initialPosition = transform.position;

            _camera.orthographicSize = INITIAL_CAMERA_VIEW_SIZE;
            StopTimePostProcessVolume.weight = 0f;

            SubscrubeToEvents();
        }
        private void OnDestroy()
            => UnsubscribeFromEvents();

        private void Update()
        {
            if (!_isPlayingAnimation)
                ReturnToNormalState();
        }

        public void FocusOn(Transform target, float speed, float duration, Action onFocused = null)
        {
            if (_focusOnCoroutine != null)
                StopCoroutine(_focusOnCoroutine);

            _focusOnCoroutine = StartCoroutine(FocusOnCoroutine(target, speed, duration, onFocused));
        }
        public void Shake(Vector4 range, float speed, float duration)
        {
            if (!_settingsData.CameraShaking)
                return;

            if(_shakeCoroutine != null)
                StopCoroutine(_shakeCoroutine);

            _shakeCoroutine = StartCoroutine(ShakeCoroutine(range, speed, duration));
        }
        
        private IEnumerator FocusOnCoroutine(Transform target, float speed, float duration, Action onFocused) 
        {
            float endTime = Time.unscaledTime + duration;
            float focusedCameraSize = 3f;
            float positionMoveSpeed = speed * 1.5f;
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, _initialPosition.z);

            _isPlayingAnimation = true;

            while(Time.unscaledTime < endTime)
            {
                StopTimePostProcessVolume.weight = Mathf.Lerp(StopTimePostProcessVolume.weight, 1, speed * Time.deltaTime);

                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, focusedCameraSize, speed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, targetPos, positionMoveSpeed * Time.deltaTime);
                yield return null;
            }
            
            onFocused?.Invoke();

            _isPlayingAnimation = false;
        }
        private IEnumerator ShakeCoroutine(Vector4 range, float speed, float duration)
        {
            float endTime = Time.time + duration;
            float changeTargetTime = 0.025f;
            float currentChangeTargetTime = changeTargetTime;
            Vector3 targetRotation = GetRandomTargetRotation();

            _isPlayingAnimation = true;

            while(Time.time < endTime)
            {
                currentChangeTargetTime -= Time.deltaTime;

                if(currentChangeTargetTime <= 0)
                {
                    targetRotation = GetRandomTargetRotation();
                    currentChangeTargetTime = changeTargetTime;
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), speed * Time.deltaTime);

                yield return null;
            }

            _isPlayingAnimation = false;

            Vector3 GetRandomTargetRotation()
            {
                return new Vector3(Random.Range(range.x, range.y), Random.Range(range.z, range.w));
            }
        }

        private void ReturnToNormalState()
        {
            float returnSizeSpeed = 3f;
            float returnPositionSpeed = 3f;
            float returnRotationSpeed = 3f;
            float returnVolumeSpeed = 3f;

            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, INITIAL_CAMERA_VIEW_SIZE, returnSizeSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, _initialPosition, returnPositionSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, returnRotationSpeed * Time.deltaTime);
            StopTimePostProcessVolume.weight = Mathf.Lerp(StopTimePostProcessVolume.weight, 0f, returnVolumeSpeed * Time.deltaTime);
        }

        private void SubscrubeToEvents()
        {
            _settingsData.OnPostProcessSettingsChanges += OnPostProcessEnableChange;

            OnPostProcessEnableChange(_settingsData.PostProccesingEnabled);
        }
        private void UnsubscribeFromEvents()
        {
            _settingsData.OnPostProcessSettingsChanges -= OnPostProcessEnableChange;
        }

        private void OnPostProcessEnableChange(bool value)
        {
            PostProcessLayer.enabled = value;
        }
    }
}
