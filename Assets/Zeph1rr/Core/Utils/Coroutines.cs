using System.Collections;
using UnityEngine;

namespace Zeph1rr.Core.Utils
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[COROUTINES]");
                    _instance = go.AddComponent<Coroutines>();
                    DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }
        private static Coroutines _instance;

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return Instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            Instance.StopCoroutine(routine);
        }
    }
}