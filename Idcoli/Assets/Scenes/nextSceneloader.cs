using UnityEngine;
using UnityEngine.SceneManagement;

public class nextSceneloader : MonoBehaviour
{
 void OnEnable()
    {
        SceneManager.LoadScene("try", LoadSceneMode.Single);
    }
}

