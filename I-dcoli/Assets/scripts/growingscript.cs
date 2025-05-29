using UnityEngine;

public class GrowingScript : MonoBehaviour
{
    [Header("Growth Settings")]
    [Tooltip("Amount to grow the soft body by (positive value, e.g., 0.5 = increase size by 0.5 units)")]
    public float growAmount = 0.5f;
    private bool hasTriggered = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("MainPlayer") && !hasTriggered)
        {
            hasTriggered = true;
            SoftBodyController sbc = collision.transform.root.GetComponent<SoftBodyController>();
            if (sbc != null)
            {
                sbc.AdjustSize(growAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("SoftBodyController not found on MainPlayer.");
            }
        }
    }
}