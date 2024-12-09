using GravityPong.Utilities;
using UnityEngine;
using UnityEngine.Windows;

namespace GravityPong
{
    public class GameSaveDataController : IGameSaveDataController
    {
        public const string FILE_NAME = "SaveData";
        public readonly string SavePath;

        public GameSaveData Data { get => _data; set => _data = value; }

        private GameSaveData _data;

        public GameSaveDataController()
        {
            if (Application.isEditor)
                SavePath = $"{Application.dataPath}/Editor/";
            else
                SavePath = $"{Application.persistentDataPath}/Save";

            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);

            Load();
        }

        public void Load()
        {
            _data = SaveUtility.LoadDataFromJson<GameSaveData>(SavePath, FILE_NAME, !Application.isEditor);
            if (_data == null)
            {
                _data = new GameSaveData();
                Save();
                return;
            }
        }

        public void Save()
        {
            SaveUtility.SaveDataToJson(SavePath, FILE_NAME, _data,!Application.isEditor);
        }
    }
}
