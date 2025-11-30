using UnityEngine;
using UnityEngine.SceneManagement;

public class AcidDrip : MonoBehaviour
{
    private bool isSinking = false;
    private float sinkDuration = 3f;
    private float sinkSpeed = 3f;
    private float sinkTimer;
    public float shrinkAmount = 0.5f;
    private bool _hasTriggerd = false;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (_hasTriggerd) return; // already handled, ignore further triggers
        //_hasTriggerd = true;

        if (collision.transform.root.CompareTag("Bacteria"))
        {
            Bacteria bacteria = collision.transform.root.GetComponent<Bacteria>();
            if (bacteria != null)
            {
                bacteria.AdjustSize(-shrinkAmount);
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("AcidPool"))
        {
            AcidWave acidWave = collision.GetComponent<AcidWave>();
            if (acidWave != null)
            {
                acidWave.AddWaveAtPoint(transform.position, 0.4f);
            }

            isSinking = true;
            sinkTimer = sinkDuration;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            if (col != null) col.enabled = false;
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }

    void Update()
    {
        if (isSinking)
        {
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;

            sinkTimer -= Time.deltaTime;
            if (sinkTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
