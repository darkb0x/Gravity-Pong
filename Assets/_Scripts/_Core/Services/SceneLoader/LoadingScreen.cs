using System;
using UnityEngine;

namespace GravityPong 
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Animation Anim;
        [SerializeField] private string ShowAnimationName;
        [SerializeField] private string HideAnimationName;

        private Action _onShown;
        private Action _onHidden;

        public void Show(Action onShown = null)
        {
            gameObject.SetActive(true);
            _onShown = onShown;
            Anim.Play(ShowAnimationName);
        }
        public void Hide(Action onHiden = null)
        {
            _onHidden = onHiden;
            Anim.Play(HideAnimationName);
        }

        private void Anim_OnShown()
        {
            _onShown?.Invoke();
        }
        private void Anim_OnHidden()
        {
            _onHidden?.Invoke();
            gameObject.SetActive(false);
        }
    }
}