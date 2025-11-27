using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager1 : MonoBehaviour
{
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject welcomeCanvas;
    [SerializeField] private GameObject bacteria;
    [SerializeField] private GameObject Audio;
    [SerializeField] private float audioDelay = 3f; // ⏱ delay before sound starts
    [SerializeField] private float fadeinduration= 1.0f;
    [SerializeField] private float fadeout = 1.0f;
    private void Start()
    {
        Audio.SetActive(true);
        bacteria.SetActive(true);
        levels.SetActive(true);
        welcomeCanvas.SetActive(false);
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
        // Fade to black
        yield return fadein.Instance.FadeOut(fadeout);

        // Enable game visuals (but not audio yet)
        levels.SetActive(true);
        bacteria.SetActive(true);
        welcomeCanvas.SetActive(false);

        // Fade back in
        yield return fadein.Instance.FadeIn(fadeinduration);

        // Wait before enabling audio
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
