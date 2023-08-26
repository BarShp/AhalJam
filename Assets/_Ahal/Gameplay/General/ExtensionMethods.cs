using System;
using System.Collections;
using UnityEngine;

public static class ExtensionMethods
{
    public static void MonoWaitForSeconds(this MonoBehaviour m, Action onComplete, float seconds)
    {
        m.StartCoroutine(WaitForSecondsCoroutine(onComplete, seconds));
    }

    private static IEnumerator WaitForSecondsCoroutine(Action onComplete, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }

    public static bool IsInLayer(this LayerMask layerMask, int layer)
    {
        return (layerMask & 1 << layer) > 0;
    }
    
    public static void SetLocalX(this Transform t, float x)
    {
        var localPosition = t.localPosition;
        localPosition.x = x;
        t.localPosition = localPosition;
    }
}
