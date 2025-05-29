using UnityEngine;

public class ShrinkerScript : MonoBehaviour
{
    [Header("Shrink Settings")]
    [Tooltip("Amount to shrink the soft body by (positive value, e.g., 0.5 = reduce size by 0.5 units)")]
    public float shrinkAmount = 0.5f;
    private bool hasTriggered = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer") && !hasTriggered)
        {
            hasTriggered = true; 
            SoftBodyController sbc = collision.transform.root.GetComponent<SoftBodyController>();
            if (sbc != null)
            {
                sbc.AdjustSize(-shrinkAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("SoftBodyController not found on MainPlayer.");
            }
        }
    }
}