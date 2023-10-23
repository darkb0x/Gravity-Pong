using System.Collections.Generic;
using UnityEngine;

namespace GravityPong.UI
{
    public class UIPanelNavigator
    {
        private GameObject _mainPanel;
        private Dictionary<UIPanelID, GameObject> _panels;

        public UIPanelNavigator(GameObject mainPanel, Dictionary<UIPanelID, GameObject> panels)
        {
            _mainPanel = mainPanel;
            _panels = panels;
        }

        public void Open(UIPanelID id)
        {
            if(_panels.TryGetValue(id, out GameObject panel))
            {
                CloseAll(false);
                panel.SetActive(true);
            }
        }
        public void CloseAll(bool enableMain = true)
        {
            foreach (var item in _panels.Keys)
                _panels[item].SetActive(false);
            
            if(enableMain)
                _mainPanel.SetActive(true);
        }
    }
}