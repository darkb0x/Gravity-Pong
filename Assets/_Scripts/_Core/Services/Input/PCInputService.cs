using UnityEngine;

namespace GravityPong
{
    public class PCInputService : IInputService
    {
        public float GetHorizontal()
            => Input.GetAxisRaw(Constants.HORIZONTAL);
    }
}