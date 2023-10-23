using UnityEngine.EventSystems;
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
