using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SoftBodyController : MonoBehaviour
{
    [Header("Soft Body Settings")]
    public GameObject pointPrefab;
    public int pointCount = 10;
    private float baseRadius = 2.5f;
    private float currentScale = 1f;
    public float sizeIncrement = 0.5f;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float springFrequency = 5f;
    public float springDamping = 0.7f;

    private List<GameObject> points = new List<GameObject>();
    private List<SpringJoint2D> joints = new List<SpringJoint2D>();
    private List<float> originalDistances = new List<float>();

    void Start()
    {
        transform.position = new Vector2(-35.4f, 18.6f);
        SpawnPointsInCircle();
        ConnectPointsWithSprings();
    }

    void SpawnPointsInCircle()
    {
        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2f / pointCount;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * baseRadius * currentScale;
            Vector2 worldPos = (Vector2)transform.position + offset;
            GameObject point = Instantiate(pointPrefab, worldPos, Quaternion.identity, transform);
            points.Add(point);
        }
    }

    void ConnectPointsWithSprings()
    {
        for (int i = 0; i < pointCount; i++)
        {
            for (int j = i + 1; j < pointCount; j++)
            {
                SpringJoint2D joint = points[i].AddComponent<SpringJoint2D>();
                joint.connectedBody = points[j].GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                float dist = Vector2.Distance(points[i].transform.position, points[j].transform.position);
                joint.distance = dist;
                joint.dampingRatio = springDamping;
                joint.frequency = springFrequency;
                joint.enableCollision = false;

                joints.Add(joint);
                originalDistances.Add(dist);
            }
        }
    }

    public void AdjustSize(float sizeChange)
    {
        currentScale += sizeChange;
        currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].distance = originalDistances[i] * currentScale;
        }

        float mass = currentScale >= maxScale ? 1.4f : 1f; foreach (var point in points)
        {
            Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
            if (rb != null) rb.mass = mass;
        }
        Debug.Log($"Scale: {currentScale}, Mass: {mass}");
    }

    public void OnGrow(InputAction.CallbackContext context)
    {
        if (context.performed) AdjustSize(sizeIncrement);
    }

    public void OnShrink(InputAction.CallbackContext context)
    {
        if (context.performed) AdjustSize(-sizeIncrement);
    }
}