using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class acidscript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer"))
        {
            Debug.Log("Player has entered the acid");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            Destroy(collision.transform.root.gameObject);
        }
    }



}
