using UnityEngine;

public class acidscript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer"))
        {
            Debug.Log("Player has entered the acid");

      
            Destroy(collision.transform.root.gameObject);
        }
    }
}
