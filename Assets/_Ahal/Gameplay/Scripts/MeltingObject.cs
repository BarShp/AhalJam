using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeltingObject : MonoBehaviour
{
    public UnityEvent isMelting = new UnityEvent();
    [SerializeField] float timeUntilDestroyed = 2f;
    protected void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(destroyMeltingObject(timeUntilDestroyed));
        }
    }

    IEnumerator destroyMeltingObject(float seconds)
    {
        isMelting.Invoke();
        
        yield return new WaitForSeconds(seconds);
        GetComponent<Collider2D>().enabled = false;
        //TODO: Start Animation
        gameObject.SetActive(false);
    }
}
