using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

namespace GravityPong.Menu
{
    public class MenuButton : UIButton
    {
        private MainMenu _mainMenu;

        public void Initialize(MainMenu mainMenu, Action onClick = null)
        {
            _mainMenu = mainMenu;

            base.Initialize(onClick);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _mainMenu.Select(this);
        }
    }
}
