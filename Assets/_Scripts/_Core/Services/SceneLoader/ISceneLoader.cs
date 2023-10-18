using System;

namespace GravityPong 
{
    public interface ISceneLoader : IService
    {
        void LoadScene(string name, Action onLoaded = null);
    }
}