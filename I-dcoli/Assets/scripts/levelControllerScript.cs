using UnityEngine;

public class levelControllerScript : MonoBehaviour
{
    [Header("Level Settings")]
    public GameObject[] levelPrefabs; // Prefabs of levels
    public Transform player;
    public Vector2[] levelStartPositions;     // Player spawn positions
    public Vector2[] levelWorldPositions;     // Where to spawn each level

    private int currentLevelIndex = 0;
    private GameObject currentLevelInstance;

    void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextLevel();
        }
    }

    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levelPrefabs.Length)
        {
            Debug.LogWarning("Invalid level index!");
            return;
        }

        // Destroy old level if exists
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        // Instantiate new level
        Vector3 spawnPosition = levelWorldPositions[index];
        currentLevelInstance = Instantiate(levelPrefabs[index], spawnPosition, Quaternion.identity);

        // Move player
        player.position = levelStartPositions[index];

        currentLevelIndex = index;

        Debug.Log($"✅ Loaded Level {index + 1}");
    }

    public void LoadNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;
        if (nextIndex < levelPrefabs.Length)
        {
            LoadLevel(nextIndex);
        }
        else
        {
            Debug.Log("🏁 No more levels!");
        }
    }
}
