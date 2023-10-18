using UnityEngine;
using System.Collections;

namespace GravityPong
{
    public interface ICoroutineRunner : IService
    {
        Coroutine RunCoroutine(IEnumerator coroutine);
    }
}