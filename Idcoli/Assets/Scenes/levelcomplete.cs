using UnityEngine;

public class levelcomplete : MonoBehaviour
{
    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeenTriggered) return;

        if (collision.transform.root.CompareTag("Bacteria"))
        {
            Bacteria bacteria = collision.transform.root.GetComponent<Bacteria>();
            Debug.Log("Level Complete Triggered");
            hasBeenTriggered = true; // Set flag to prevent retriggering
            FindAnyObjectByType<GameManager1>().GoToNextLevel();
        }
    }

    // Optional: Reset the trigger if you need to reuse this object
    public void ResetTrigger()
    {
        hasBeenTriggered = false;
    }
}