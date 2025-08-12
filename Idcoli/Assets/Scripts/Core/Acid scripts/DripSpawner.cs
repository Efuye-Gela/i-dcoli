using UnityEngine;

public class DripSpawner : MonoBehaviour
{
    public GameObject dripPrefab;
    public float dripInterval = 1.5f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= dripInterval)
        {
            Instantiate(dripPrefab, transform.position, Quaternion.identity);
            timer = 0f;
        }
    }
}
