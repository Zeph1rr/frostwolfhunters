using System;
using System.Collections;
using UnityEngine;

namespace Zeph1rr.Core.Monos
{
    public abstract class Mono : MonoBehaviour
    {
        public Transform Transform => transform;
        public new Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return base.StartCoroutine(coroutine);
        }

        public new void StopAllCoroutines()
        {
            base.StopAllCoroutines();
        }

        public new void StopCoroutine(Coroutine coroutine)
        {
            base.StopCoroutine(coroutine);
        }

        public IEnumerator SmoothUpdateValue(Action<float> updateAction, float duration, float startValue, float targetValue, Action callback = null)
        {
            float elapsedTime = 0;
            while (elapsedTime <= duration)
            {
                elapsedTime += Time.deltaTime;
                var newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
                updateAction.Invoke(newValue);
                yield return null;
            }
            updateAction.Invoke(targetValue);
            callback?.Invoke();
        }
    }
}