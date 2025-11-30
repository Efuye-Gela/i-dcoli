using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class fjdkvcmx : MonoBehaviour, IPointerClickHandler
{
    // Trigger your scene load
    public void Onpress()
    {
        SceneManager.LoadScene(0);
    }

    // Called by mouse click OR touch on WebGL/mobile
    public void OnPointerClick(PointerEventData eventData)
    {
        Onpress();
        Debug.Log("Scene loading triggered.");
    }
}
