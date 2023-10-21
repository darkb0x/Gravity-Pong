using UnityEngine;

namespace GravityPong
{
    public class UIPanelNavigator : MonoBehaviour
    {
        private GameObject _mainPanel;
        private GameObject[] _panelsList;

        public void Initialize(GameObject mainPanel, GameObject[] panelsList)
        {
            _mainPanel = mainPanel;
            _panelsList = panelsList;
        }

        public void Open(GameObject panel)
        {
            CloseAll(false);
            panel.SetActive(true);
        }
        public void CloseAll(bool enableMain = true)
        {
            foreach (var item in _panelsList)
                item.SetActive(false);
            
            if(enableMain)
                _mainPanel.SetActive(true);
        }
    }
}