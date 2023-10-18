using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

namespace GravityPong.Menu
{
    public class MenuButton : UIButton
    {
        private MainMenu _mainMenu;

        public void Initialize(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
            
            Deselect();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _mainMenu.Select(this);
        }
    }
}
