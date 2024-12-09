using GravityPong.UI;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GravityPong.Menu
{
    public class GameModeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _classicHighscoreText;
        [SerializeField] private TMP_Text _arcadeHighscoreText;

        [Header("Btns")]
        [SerializeField] private UIButton _closeButton;
        [Space]
        [SerializeField] private UIButton _classicGameModeButton;
        [SerializeField] private UIButton _arcadeGameModeButton;

        private UIPanelNavigator _uiPanelNavigator;
        private ISceneLoader _sceneLoader;
        private IGameSaveDataController _saveData;

        public void Initialize(UIPanelNavigator panelNavigator, ISceneLoader sceneLoader, IGameSaveDataController saveData)
        {
            _sceneLoader = sceneLoader;
            _uiPanelNavigator = panelNavigator;
            _saveData = saveData;

            InitializeButtons();
            InitializeHighscores();
        }

        private void InitializeButtons()
        {
            _closeButton.Initialize(Close);
            _classicGameModeButton.Initialize(StartClassicGameMode);
            _arcadeGameModeButton.Initialize(StartArcadeGameMode);

            _arcadeGameModeButton.Button.interactable = false;
        }
        private void InitializeHighscores()
        {
            var save = _saveData.Data;
            _classicHighscoreText.text = $"Highscore: {save.ClassicGameHighscore}";
            _arcadeHighscoreText.text = $"Highscore: {save.ArcadeGameHighscore}";
        }

        private void Close()
        {
            _uiPanelNavigator.CloseAll();

            _closeButton.Deselect();
            _classicGameModeButton.Deselect();
            _arcadeGameModeButton.Deselect();
        }

        private void StartClassicGameMode()
        {
            _saveData.Data.CurrentGameMode = 0;
            DisableButtons();
            _sceneLoader.LoadScene(Constants.Scenes.CLASSIC_GAME_SCENE_NAME);
        }
        private void StartArcadeGameMode()
        {
            _saveData.Data.CurrentGameMode = 1;
            DisableButtons();
            _sceneLoader.LoadScene(Constants.Scenes.ARCADE_GAME_SCENE_NAME);
        }

        private void DisableButtons()
        {
            _classicGameModeButton.Button.interactable = false;
            _arcadeGameModeButton.Button.interactable = false;
            _closeButton.Button.interactable = false;
        }
    }
}
