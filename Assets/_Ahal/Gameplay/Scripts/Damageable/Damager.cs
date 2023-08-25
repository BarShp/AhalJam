using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent<DamageableComponent>(out var damageableComponent)) return;
        damageableComponent.OnDamage();
    }
}
