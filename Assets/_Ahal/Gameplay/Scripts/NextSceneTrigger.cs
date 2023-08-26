using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneTrigger : MonoBehaviour
{
    [SerializeField] int nextSceneIndex;
    [SerializeField] float delayTime = 2.5f;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player")
        {
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;            
            other.GetComponent<PlayerMovement2D>().disableControls = true;
            this.MonoWaitForSeconds(
                () => FindObjectOfType<SceneController>().MoveScene(nextSceneIndex),
                delayTime
            );
        }
    }

}
