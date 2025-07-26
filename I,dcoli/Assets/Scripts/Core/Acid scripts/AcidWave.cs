using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
public class AcidWave : MonoBehaviour
{
    public SpriteShapeController spriteShape;
    public float waveSpeed = 2f;
    public float waveHeight = 0.2f;
    public float waveFrequency = 1f;

    [Header("Top Wave Point Range")]
    public int startPointIndex = 0;
    public int endPointIndex = 5;

    [Header("Splash Settings")]
    public float splashAmplitude = 0.3f;
    public float splashDamping = 1.5f;

    private Spline spline;
    private Vector3[] originalPositions;
    private float[] splashOffsets;

    void Start()
    {
        spline = spriteShape.spline;
        int count = spline.GetPointCount();

        originalPositions = new Vector3[count];
        splashOffsets = new float[count];

        for (int i = 0; i < count; i++)
        {
            originalPositions[i] = spline.GetPosition(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            Vector3 pos = originalPositions[i];

            if (i >= startPointIndex && i <= endPointIndex)
            {
                // Apply base wave + splash offset
                float wave = Mathf.Sin(Time.time * waveSpeed + i * waveFrequency) * waveHeight;
                splashOffsets[i] = Mathf.Lerp(splashOffsets[i], 0f, Time.deltaTime * splashDamping);
                pos.y += wave + splashOffsets[i];
            }

            spline.SetPosition(i, pos);
        }
        //
        spriteShape.RefreshSpriteShape();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.CompareTag("Bacteria"))
        {
            Destroy(collision.transform.root.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void AddWaveAtPoint(Vector3 worldPos, float amplitude = 0.3f)
    {
        int closestIndex = -1;
        float closestDist = Mathf.Infinity;

        for (int i = startPointIndex; i <= endPointIndex; i++)
        {
            Vector3 pointWorld = transform.TransformPoint(originalPositions[i]);
            float dist = Mathf.Abs(pointWorld.x - worldPos.x);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        if (closestIndex != -1)
        {
            splashOffsets[closestIndex] += amplitude;
        }
    }
}
