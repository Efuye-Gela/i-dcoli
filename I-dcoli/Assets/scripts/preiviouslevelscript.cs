using UnityEngine;

public class preiviouslevelscript : MonoBehaviour
{
    public  LevelManager levelController;
    private bool istriggered =false; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        istriggered = true;
        if (istriggered) return;
        if(collision.transform.root.CompareTag("MainPlayer"))
        {
            Debug.Log(" previouslevel trigger");
            istriggered = false;
            Debug.Log("Player has entered the previous level trigger");
            levelController.previousLevel();
            
        }


    }
}
