using UnityEngine;

public class levelmanager : MonoBehaviour
{

    public GameObject[] levels;
    public GameObject circle;
    public GameObject currentLevel;
    public int currentIndex = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onlevel(currentIndex);

    }
    public void Nextlevel()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel);
            currentIndex++;
        }

        if (currentIndex < levels.Length)
        {
            onlevel(currentIndex);
        }
        else
        {
            Debug.Log("game won");
        }
    }
    void onlevel(int index)
    {
        if (currentIndex == 0)
        {
            
            circle.transform.position = new Vector3(50f, -50f, 0f);
            Vector3 spawnPosition = new Vector3(156f, 35.3f, 0f);
            currentLevel = Instantiate(levels[index], spawnPosition, Quaternion.identity);
            currentLevel.SetActive(true);
        }
        else
        {
            circle.transform.position = new Vector3(7.5f, -1.8f, 0f);
            Vector3 spawnPosition = new Vector3(20.5f, 6.29f, 0f);
            currentLevel = Instantiate(levels[index], spawnPosition, Quaternion.identity);
        }
    }

}
