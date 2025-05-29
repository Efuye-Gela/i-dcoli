using UnityEngine;

public class LevelWonScript : MonoBehaviour
{
    private LevelManager levelManager;
    private bool hasTriggered = false; // Add flag to prevent multiple triggers

    [Header("Transition Settings")]
    [Tooltip("Delay in seconds before loading the next level")]
    public float transitionDelay = 1.0f;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer") && !hasTriggered)
        {
            Debug.Log("Player reached level exit trigger.");
            hasTriggered = true; 

            if (levelManager != null)
            {
                StartCoroutine(TransitionToNextLevel());
            }
            else
            {
                Debug.LogError("Cannot load next level because levelManager is null.");
            }
        }
    }

    private System.Collections.IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(transitionDelay);
        levelManager.NextLevel();
    }
}