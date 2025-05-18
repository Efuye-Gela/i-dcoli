using UnityEngine;

public class shrinkerscript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer"))
        {
            SoftBodyController sbc = collision.transform.root.GetComponent<SoftBodyController>();
            if (sbc != null)
            {
                sbc.shrinkJoints(); 
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("SoftBodyController not found on MainPlayer.");
            }
        }
    }
}
