using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableComponent : MonoBehaviour
{
    public abstract void OnDamage();
    
    #if UNITY_EDITOR
    // Editor Tools
    
    [ContextMenu("DamageTest")]
    private void DamageTest()
    {
        OnDamage();
    } 
    #endif
}
