using UnityEngine;

public class levelwonscript : MonoBehaviour
{
    private levelControllerScript levelController;

    private void Start()
    {
        // Find the first active instance of levelControllerScript in the scene
        levelController = FindFirstObjectByType<levelControllerScript>();
        if (levelController == null)
        {
            Debug.LogError("LevelControllerScript not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer"))
        {
            Debug.Log("Player reached level exit trigger.");

            if (levelController != null)
            {
                levelController.LoadNextLevel();
            }
            else
            {
                Debug.LogError("Cannot load next level because levelController is null.");
            }
        }
    }
}
