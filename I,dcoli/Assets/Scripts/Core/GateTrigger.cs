using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D other)
    { 
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null && Mathf.Approximately(rb.mass, 1.4f))
        {gameManager.OpenGate();
           Debug.Log("Gate Triggered by object with mass 2");
        }
    }
}
