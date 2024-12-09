using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GravityPong
{
    public interface IGameSaveDataController : IService
    {
        GameSaveData Data { get; set; }

        void Save();
        void Load();
    }
}
