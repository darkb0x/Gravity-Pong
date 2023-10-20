using UnityEngine;

namespace GravityPong
{
    public class MobileInputService : IInputService
    {
        public float GetHorizontal()
        {
            float horizontal = 0f;
            
            if(Input.touchCount > 0)
            {
                foreach (var touch in Input.touches)
                {
                    float x = Camera.main.ScreenToViewportPoint(touch.position).x;

                    if (x < .5f) // left 
                    {
                        horizontal = -1;
                    }
                    else // right
                    {
                        horizontal = 1;
                    }
                }
            }

            return horizontal;
        }
    }
}