using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager1 : MonoBehaviour
{
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject welcomeCanvas;
    [SerializeField] private GameObject bacteria;
    [SerializeField] private GameObject Audio;
    [SerializeField] private float audioDelay = 3f;
    [SerializeField] private float fadeinduration = 1.0f;
    [SerializeField] private float fadeout = 1.0f;

    [Header("Level Management")]
    [SerializeField] private List<GameObject> levelPrefabs = new List<GameObject>();
    [SerializeField] private List<Transform> playerSpawnPositions = new List<Transform>();

    [Header("UI Canvases")]
    [SerializeField] private GameObject levelCompleteCanvas;

    private int currentLevelIndex = 0;
    private int currentIndex;
    private GameObject currentBacteria; // Reference to the current bacteria instance
    private JoystickMove[] joystickMoveScripts; // Reference to joystick scripts

    private void Start()
    {
        Audio.SetActive(true);
        welcomeCanvas.SetActive(false);

        // Ensure level complete canvas is hidden at start
        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(false);

        levels.SetActive(true);
        SetupLevels();

        // Find all joystick move scripts
        joystickMoveScripts = FindObjectsOfType<JoystickMove>();
    }

    // NEW METHOD: Show level complete canvas
    public void ShowLevelCompleteCanvas()
    {
        if (levelCompleteCanvas != null)
        {
            levelCompleteCanvas.SetActive(true);
            Debug.Log("Level Complete Canvas Shown");

            // Optional: Pause game or disable player input
            // Time.timeScale = 0f;
        }
        else
        {
            Debug.LogWarning("Level Complete Canvas is not assigned!");
            // Fallback: go directly to next level
            GoToNextLevel();
        }
    }

    // UPDATED METHOD: Now called from the "Next" button
    public void GoToNextLevel()
    {
        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(false);

        // Optional: Resume game if paused
        // Time.timeScale = 1f;

        if (currentLevelIndex + 1 < levelPrefabs.Count)
        {
            StartCoroutine(NextLevelRoutine());
        }
        else
        {
            Debug.Log("Game Completed!");
            // Optional: Show game completed screen
        }
    }

    private IEnumerator NextLevelRoutine()
    {
        // Skip fade effects entirely for now
        // yield return fadein.Instance.FadeOut(fadeout);

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

        // Destroy old bacteria and instantiate new one at spawn position
        if (bacteria != null && currentLevelIndex < playerSpawnPositions.Count && playerSpawnPositions[currentLevelIndex] != null)
        {
            // Clear joystick references before destroying
            ClearJoystickReferences();

            // Destroy the current bacteria instance
            if (currentBacteria != null)
            {
                Destroy(currentBacteria);
            }

            // Instantiate new bacteria at the spawn position
            currentBacteria = Instantiate(bacteria,
                playerSpawnPositions[currentLevelIndex].position,
                playerSpawnPositions[currentLevelIndex].rotation);

            currentBacteria.SetActive(true);

            // Update joystick references with new bacteria
            UpdateJoystickReferences();

            Debug.Log($"New bacteria instantiated at: {playerSpawnPositions[currentLevelIndex].position}");
        }

        // Skip fade effects entirely for now
        // yield return fadein.Instance.FadeIn(fadeinduration);

        Debug.Log($"Now in Level {currentLevelIndex + 1}");

        yield return null; // Required for coroutine
    }

    // NEW METHOD: Restart current level
    public void RestartCurrentLevel()
    {
        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine()
    {
        Debug.Log($"Restarting Level {currentLevelIndex + 1}");

        // Optional: Add fade effects here
        // yield return fadein.Instance.FadeOut(fadeout);

        // Deactivate and reactivate current level to reset it
        if (levelPrefabs[currentLevelIndex] != null)
        {
            levelPrefabs[currentLevelIndex].SetActive(false);
            levelPrefabs[currentLevelIndex].SetActive(true);
        }

        // Destroy old bacteria and instantiate new one at current level's spawn position
        if (bacteria != null && currentLevelIndex < playerSpawnPositions.Count && playerSpawnPositions[currentLevelIndex] != null)
        {
            // Clear joystick references before destroying
            ClearJoystickReferences();

            // Destroy the current bacteria instance
            if (currentBacteria != null)
            {
                Destroy(currentBacteria);
            }

            // Instantiate new bacteria at the current level's spawn position
            currentBacteria = Instantiate(bacteria,
                playerSpawnPositions[currentLevelIndex].position,
                playerSpawnPositions[currentLevelIndex].rotation);

            currentBacteria.SetActive(true);

            // Update joystick references with new bacteria
            UpdateJoystickReferences();

            Debug.Log($"Bacteria restarted at: {playerSpawnPositions[currentLevelIndex].position}");
        }

        if (levelCompleteCanvas != null && levelCompleteCanvas.activeSelf)
        {
            levelCompleteCanvas.SetActive(false);
        }

        

        Debug.Log($"Level {currentLevelIndex + 1} restarted successfully");

        yield return null;
    }

    // NEW METHOD: Clear joystick references before destroying bacteria
    private void ClearJoystickReferences()
    {
        if (joystickMoveScripts != null)
        {
            foreach (var joystick in joystickMoveScripts)
            {
                if (joystick != null)
                {
                    // Use reflection or public method to clear the reference
                    var rigidbodyField = joystick.GetType().GetField("rb",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (rigidbodyField != null)
                    {
                        rigidbodyField.SetValue(joystick, null);
                    }
                }
            }
        }
    }

    // NEW METHOD: Update joystick references with new bacteria
    private void UpdateJoystickReferences()
    {
        if (joystickMoveScripts != null && currentBacteria != null)
        {
            Rigidbody2D newRigidbody = currentBacteria.GetComponent<Rigidbody2D>();

            foreach (var joystick in joystickMoveScripts)
            {
                if (joystick != null)
                {
                    // Use reflection to set the rigidbody reference
                    var rigidbodyField = joystick.GetType().GetField("rb",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (rigidbodyField != null && newRigidbody != null)
                    {
                        rigidbodyField.SetValue(joystick, newRigidbody);
                    }
                }
            }
        }
    }

    private void DisableOtherBacteria()
    {
        GameObject[] allBacteria = GameObject.FindGameObjectsWithTag("Bacteria");

        foreach (GameObject otherBacteria in allBacteria)
        {
            if (otherBacteria != currentBacteria)
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

        // Instantiate initial bacteria at first spawn position
        if (bacteria != null && playerSpawnPositions.Count > 0 && playerSpawnPositions[0] != null)
        {
            // Destroy any existing bacteria first
            if (currentBacteria != null)
            {
                Destroy(currentBacteria);
            }

            // Instantiate the initial bacteria
            currentBacteria = Instantiate(bacteria,
                playerSpawnPositions[0].position,
                playerSpawnPositions[0].rotation);

            currentBacteria.SetActive(true);

            // Update joystick references with initial bacteria
            UpdateJoystickReferences();

            Debug.Log($"Initial bacteria instantiated at: {playerSpawnPositions[0].position}");
        }
    }

    // Add this debug method to check positions
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentBacteria != null)
            {
                Debug.Log($"Current bacteria position: {currentBacteria.transform.position}");
            }
            if (currentLevelIndex < playerSpawnPositions.Count && playerSpawnPositions[currentLevelIndex] != null)
            {
                Debug.Log($"Current spawn position: {playerSpawnPositions[currentLevelIndex].position}");
            }
        }
    }

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
        if (currentBacteria != null) currentBacteria.SetActive(true);
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

    // Updated RestartGame method - now calls RestartCurrentLevel instead of reloading scene
    public void RestartGame()
    {
        Audio.SetActive(false);
        RestartCurrentLevel();
        Audio.SetActive(true); // Restart audio after reset
    }
}