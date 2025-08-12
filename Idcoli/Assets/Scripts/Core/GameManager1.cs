using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject welcomeCanvas;
    [SerializeField] private GameObject bacteria;
    [SerializeField] private GameObject Audio; 
    private void Start()
    {
        if (levels == null)
        {
            Debug.LogError("Levels GameObject is not assigned in the inspector.");
            return;
        }
        if (welcomeCanvas == null)
        {
            Debug.LogError("Welcome Canvas GameObject is not assigned in the inspector.");
            return;
        }
        if (bacteria == null)
        {
            Debug.LogError("Bacteria GameObject is not assigned in the inspector.");
            return;
        }
        Audio.SetActive(false);
        bacteria.SetActive(false);
        levels.SetActive(false);
        welcomeCanvas.SetActive(true);
    }

    public void StartGame()
    {
        if (levels != null && welcomeCanvas != null)
        {
            Audio.SetActive(true);
            levels.SetActive(true);
            bacteria.SetActive(true);
            welcomeCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("Levels or Welcome Canvas GameObject is not assigned.");
        }
    }   
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    public void RestartGame()
    {
        if (levels != null && welcomeCanvas != null)
        {
            Audio.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        else
        {
            Debug.LogError("Levels or Welcome Canvas GameObject is not assigned.");
        }
    }





}