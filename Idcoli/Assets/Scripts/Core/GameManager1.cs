using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager1 : MonoBehaviour
{
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject welcomeCanvas;
    [SerializeField] private GameObject bacteria; // Your main player bacteria
    [SerializeField] private GameObject Audio;
    [SerializeField] private float audioDelay = 3f;
    [SerializeField] private float fadeinduration = 1.0f;
    [SerializeField] private float fadeout = 1.0f;

    [Header("Level Management")]
    [SerializeField] private List<GameObject> levelPrefabs = new List<GameObject>();
    [SerializeField] private List<Transform> playerSpawnPositions = new List<Transform>();
    private int currentLevelIndex = 0;

    private void Start()
    {
        Audio.SetActive(true);
        welcomeCanvas.SetActive(false);

        // Make sure only YOUR bacteria is active, not any others
        if (bacteria != null)
        {
            bacteria.SetActive(true);
            // Disable any other bacteria in the scene
            DisableOtherBacteria();
        }

        levels.SetActive(true);
        SetupLevels();
    }

    private void DisableOtherBacteria()
    {
        // Find all bacteria objects in the scene
        GameObject[] allBacteria = GameObject.FindGameObjectsWithTag("Bacteria");

        foreach (GameObject otherBacteria in allBacteria)
        {
            // Only keep YOUR assigned bacteria active, disable all others
            if (otherBacteria != bacteria)
            {
                otherBacteria.SetActive(false);
                Debug.Log("Disabled duplicate bacteria: " + otherBacteria.name);
            }
        }
    }

    private void SetupLevels()
    {
        for (int i = 0; i < levelPrefabs.Count; i++)
        {
            if (levelPrefabs[i] != null)
            {
                levelPrefabs[i].SetActive(i == 0);
            }
        }

        // Set player bacteria to first level position
        if (bacteria != null && playerSpawnPositions.Count > 0)
        {
            bacteria.transform.position = playerSpawnPositions[0].position;
            bacteria.transform.rotation = playerSpawnPositions[0].rotation;
            Debug.Log($"Player set to position: {playerSpawnPositions[0].position}");
        }
    }

    public void GoToNextLevel()
    {
        if (currentLevelIndex + 1 < levelPrefabs.Count)
        {
            // Deactivate current level
            if (levelPrefabs[currentLevelIndex] != null)
            {
                levelPrefabs[currentLevelIndex].SetActive(false);
            }

            currentLevelIndex++;

            // Activate next level
            if (levelPrefabs[currentLevelIndex] != null)
            {
                levelPrefabs[currentLevelIndex].SetActive(true);
            }

            // Move player to new level position
            if (bacteria != null && currentLevelIndex < playerSpawnPositions.Count)
            {
                bacteria.transform.position = playerSpawnPositions[currentLevelIndex].position;
                bacteria.transform.rotation = playerSpawnPositions[currentLevelIndex].rotation;

                // Clean up any new bacteria that might have spawned in the new level
                DisableOtherBacteria();
                Debug.Log($"Player moved to: {playerSpawnPositions[currentLevelIndex].position}");
            }

            Debug.Log($"Now in Level {currentLevelIndex + 1}");
        }
        else
        {
            Debug.Log("Game Completed!");
        }
    }

    // Add this debug method to check positions
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (bacteria != null)
            {
                Debug.Log($"Player position: {bacteria.transform.position}");
            }
            if (currentLevelIndex < playerSpawnPositions.Count && playerSpawnPositions[currentLevelIndex] != null)
            {
                Debug.Log($"Spawn position: {playerSpawnPositions[currentLevelIndex].position}");
            }
        }
    }

    // YOUR ORIGINAL METHODS
    public void StartGame()
    {
        if (levels != null && welcomeCanvas != null)
        {
            StartCoroutine(StartGameRoutine());
        }
        else
        {
            Debug.LogError("Levels or Welcome Canvas GameObject is not assigned.");
        }
    }

    private IEnumerator StartGameRoutine()
    {
        yield return fadein.Instance.FadeOut(fadeout);

        levels.SetActive(true);
        if (bacteria != null) bacteria.SetActive(true);
        welcomeCanvas.SetActive(false);

        yield return fadein.Instance.FadeIn(fadeinduration);

        yield return new WaitForSeconds(audioDelay);
        Audio.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void RestartGame()
    {
        Audio.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}