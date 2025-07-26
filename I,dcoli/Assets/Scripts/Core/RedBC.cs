using UnityEngine;

public class RedBC : MonoBehaviour
{
    [Header("Growth Settings")]
    [Tooltip("Amount to grow the soft body by (positive value, e.g., 0.5 = increase size by 0.5 units)")]
    public float growAmount = 0.5f;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("Bacteria") && !hasTriggered)
        {
            hasTriggered = true;
            Bacteria bacteria = collision.transform.root.GetComponent<Bacteria>();
            if (bacteria != null)
            {
                bacteria.AdjustSize(growAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Bacteria not found on MainPlayer.");
            }
        }
    }
}
