using UnityEngine;

public class WhiteBC : MonoBehaviour
{
    [Header("Shrink Settings")]
    [Tooltip("Amount to shrink the soft body by (positive value, e.g., 0.5 = reduce size by 0.5 units)")]
    public float shrinkAmount = 0.5f;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("Bacteria") && !hasTriggered)
        {
            hasTriggered = true;
            Bacteria bacteria = collision.transform.root.GetComponent<Bacteria>();
            if (bacteria != null)
            {
                bacteria.AdjustSize(-shrinkAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Bacteria not found on MainPlayer.");
            }
        }
    }
}
