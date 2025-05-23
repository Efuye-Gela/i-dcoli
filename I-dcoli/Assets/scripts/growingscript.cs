using UnityEngine;
using static SoftBodyController;

public class growingscript : MonoBehaviour
{
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer"))
        {
            SoftBodyController sbc = collision.transform.root.GetComponent<SoftBodyController>();
            if (sbc != null)
            {
                sbc.SetJointState(JointSizeState.Large); ;
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("SoftBodyController not found on MainPlayer.");
            }
        }
    }


}
