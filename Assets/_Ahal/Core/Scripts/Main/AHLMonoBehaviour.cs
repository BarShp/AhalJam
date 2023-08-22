using System;
using System.Collections;
using AHL.Core.Events;
using UnityEngine;

namespace AHL.Core.Main
{
    public class AHLMonoBehaviour : MonoBehaviour
    {
        #region Cached Transform
        private Transform cachedTransform;
        private bool didCacheTransform = false;
        public new Transform transform 
        {
            get 
            {
                // Using a flag instead of comparing to null because in Unity this
                // is a custom operator.
                if (didCacheTransform) return cachedTransform;
                
                didCacheTransform = true;
                cachedTransform = GetComponent<Transform>();
                return cachedTransform;
            }
        }
        
        #endregion
        
        protected AHLManager AHLManager;

        public virtual void Init(AHLManager ahlManager)
        {
            AHLManager = ahlManager;
        }

        protected void InvokeEvent(IAHLEvent ahlEvent)
        {
            AHLManager?.Events?.InvokeAHLEvent(ahlEvent);
        }

        protected void AddListener<T>(Action<T> action, int priority = 100) where T : IAHLEvent =>
            AHLManager.Events.AddEventListener(action, priority);

        protected void RemoveListener<T>(Action<T> action) where T : IAHLEvent
        {
            if (AHLManager is {Events: not null})
            {
                AHLManager.Events.RemoveEventListener(action);
            }
        }

        public Coroutine WaitForTimeSeconds(float waitTime, Action onComplete)
        {
            return StartCoroutine(WaitForTimeCoroutine(waitTime, onComplete));
        }

        public Coroutine WaitForRealTimeSeconds(float waitTime, Action onComplete)
        {
            return StartCoroutine(WaitForRealTimeCoroutine(waitTime, onComplete));
        }

        public Coroutine WaitForFrame(Action onComplete)
        {
            return StartCoroutine(WaitForFrameCoroutine(onComplete));
        }

        public Coroutine WaitForEndOfFrame(Action onComplete)
        {
            return StartCoroutine(WaitForEndOfFrameCoroutine(onComplete));
        }

        private IEnumerator WaitForTimeCoroutine(float waitTime, Action onComplete)
        {
            yield return new WaitForSeconds(waitTime);
            onComplete?.Invoke();
        }

        private IEnumerator WaitForRealTimeCoroutine(float waitTime, Action onComplete)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            onComplete?.Invoke();
        }

        private IEnumerator WaitForFrameCoroutine(Action onComplete)
        {
            yield return null;
            onComplete?.Invoke();
        }

        private IEnumerator WaitForEndOfFrameCoroutine(Action onComplete)
        {
            yield return new WaitForEndOfFrame();
            onComplete?.Invoke();
        }

        protected float LoopSpeed = 1;

        protected Coroutine WaitForChangeableTime(float waitTime, Action onComplete)
        {
            return StartCoroutine(WaitForChangeableTimeCoroutine(waitTime, onComplete));
        }

        private IEnumerator WaitForChangeableTimeCoroutine(float waitTime, Action onComplete)
        {
            float time = 0;

            while (true)
            {
                yield return null;
                time += UnityEngine.Time.deltaTime * LoopSpeed;

                if (time >= waitTime)
                {
                    onComplete?.Invoke();
                    break;
                }
            }
        }
    }
}