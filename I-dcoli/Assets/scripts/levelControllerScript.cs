using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;
    public GameObject circle;
    public GameObject currentLevel;
    public int currentIndex = 0;

    void Start()
    {
        Debug.Log("Starting with currentIndex: " + currentIndex);
        OnLevel(currentIndex);
    }

    public void NextLevel()
    {
        Debug.Log("Attempting to load next level. Current Index: " + currentIndex);
        if (currentLevel != null)
        {
            Destroy(currentLevel);
        }

        if (currentIndex < levels.Length - 1) 
        {
            currentIndex++;
            Debug.Log("Incremented to Index: " + currentIndex);
            OnLevel(currentIndex);
        }
        else
        {
            Debug.Log("Game Won");
        }
    }

    void OnLevel(int index)
    {
        Debug.Log("Loading Level at Index: " + index);
        if (index == 0)
        {
            circle.transform.position = new Vector3(-33.3f, 18.8f, 0f);
            Vector3 spawnPosition = new Vector3(10.4f, 6.29f, 0f);
            currentLevel = Instantiate(levels[index], spawnPosition, Quaternion.identity);
        }
        else if (index == 1)
        {
            circle.transform.position = new Vector3(-115f, 8f, 0f);
            Vector3 spawnPosition = new Vector3(27.8f, 61.8f, 0f);
            currentLevel = Instantiate(levels[index], spawnPosition, Quaternion.identity);
        }
        else if (index == 2)
        {
            circle.transform.position = new Vector3(-181f, 70f, 0f);
            Vector3 spawnPosition = new Vector3(23.1f, 65.7f, 0f);
            currentLevel = Instantiate(levels[index], spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Invalid level index: " + index);
        }
    }
}