using UnityEngine;
using System.IO;

namespace GravityPong.Utilities
{
    public static class SaveUtility
    {
        public const string FILE_EXTENSION = "json";
        private const string ENCRYPT_KEY = "217702026";

        /// <summary>
        /// Using for save files to json
        /// </summary>
        /// <param name="filePath">path in format ".../"</param>
        /// <param name="fileName">file name</param>
        /// <param name="data">data for saving</param>
        /// <param name="encrypt">if you want encrypt data, enable this parameter</param>
        public static void SaveDataToJson(string filePath, string fileName, object data, bool encrypt = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("SaveDataUtility.cs | LoadDataFromJson(string, string, bool) | filePath is empty");
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("SaveDataUtility.cs | LoadDataFromJson(string, string, bool) | fileName is empty");
                return;
            }

            string path = $"{filePath}{fileName}.{FILE_EXTENSION}";

            string json = JsonUtility.ToJson(data, true);

            if (encrypt)
            {
                File.WriteAllText(path, EncryptDecrypt(json));
            }
            else
            {
                File.WriteAllText(path, json);
            }
        }

        /// <summary>
        /// Using for load files from json
        /// </summary>
        /// <param name="filePath">path in format ".../"</param>
        /// <param name="fileName">file name</param>
        /// <param name="ecrypt">if data is encrypted, you should turn on this parameter</param>
        /// <returns>loaded data</returns>
        public static T LoadDataFromJson<T>(string filePath, string fileName, bool ecrypt = false) where T : class
        {
            if (string.IsNullOrEmpty(filePath) )
            {
                Debug.LogError("SaveDataUtility.cs | LoadDataFromJson(string, string, bool) | filePath is empty");
                return null;
            }
            if(string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("SaveDataUtility.cs | LoadDataFromJson(string, string, bool) | fileName is empty");
                return null;
            }

            string path = $"{filePath}{fileName}.{FILE_EXTENSION}";
            if (!File.Exists(path))
            {
                Debug.LogWarning($"SaveDataUtility.cs | LoadDataFromJson(string, string, bool) | file at path '{path}', is not exist.");
                return null;
            }

            string json = File.ReadAllText(path);

            if (ecrypt)
            {
                return JsonUtility.FromJson<T>(EncryptDecrypt(json));
            }
            else
            {
                return JsonUtility.FromJson<T>(json);
            }
        }

        public static string EncryptDecrypt(string data)
        {
            string result = "";

            for (int i = 0; i < data.Length; i++)
            {
                result += (char)(data[i] ^ ENCRYPT_KEY[i % ENCRYPT_KEY.Length]);
            }

            return result;
        }
    }
}
