using UnityEngine;

public class pausescript : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject joystic;
    [SerializeField] private GameManager1 gameManager;
    private bool isPaused = false;

    void Start()
    {
        pauseCanvas.SetActive(false); // hide at start
        image.SetActive(false);
    }

    void Update()
    {
        // Press ESC or P to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        image.SetActive(true);
        menu.SetActive(false);
        joystic.SetActive(!isPaused);

        pauseCanvas.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }
    public void restartgame()
    {
        pauseCanvas.SetActive(false);
        image.SetActive(false);
        gameManager.RestartCurrentLevel();

    }
    public void ResumeGame()
    {
        isPaused = false;
        image.SetActive(false);
        menu.SetActive(true);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        joystic.SetActive(true);
        menu.SetActive(false);
    }
}
