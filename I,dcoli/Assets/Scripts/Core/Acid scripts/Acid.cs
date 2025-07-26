using UnityEngine;
using UnityEngine.SceneManagement;

public class AcidDrip : MonoBehaviour
{
    private bool isSinking = false;
    private float sinkDuration = 3f;
    private float sinkSpeed = 3f;
    private float sinkTimer;

    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("Bacteria"))
        {
            Debug.Log("Player hit by acid!");
            Destroy(collision.transform.root.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
