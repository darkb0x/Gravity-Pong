using UnityEngine;

namespace GravityPong.Infrasturcture
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        public virtual void StartInitializing()
        {
            InitializeEntries();
        }
        protected abstract void InitializeEntries();
    }
}