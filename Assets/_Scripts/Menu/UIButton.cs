using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace GravityPong.Menu
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        protected const float DISABLED_TEXT_ALPHA = 0.2f;
        protected const float ENABLED_TEXT_ALPHA = 1.0f;

        public Button Button;
        [Space]
        [SerializeField] protected string Text;
        [SerializeField] protected TMP_Text ButtonText;

        public virtual void Initialize(Action onClick = null)
        {
            if (onClick != null)
                Button.onClick.AddListener(new UnityEngine.Events.UnityAction(onClick));

            Deselect();
        }

        public virtual void Select()
        {
            ButtonText.text = $"[{Text}]";
            ButtonText.alpha = ENABLED_TEXT_ALPHA;
        }
        public virtual void Deselect()
        {
            ButtonText.text = Text;
            ButtonText.alpha = DISABLED_TEXT_ALPHA;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if(Button.interactable)
                Select();
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if(Button.interactable)
                Deselect();
        }
    }
}