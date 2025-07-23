using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bacteria : MonoBehaviour
{

    [Header("Game_Objects")]
    [SerializeField] private GameObject porsPrefab;
    [SerializeField] private GameObject centerPrefab;
    private GameObject centerPoint;
    [Header("Properties")]
    [SerializeField] private int porsCount = 10;
    [SerializeField] private float baseRadius = 1f;
    [SerializeField] private float springFrequency = 5f;
    [SerializeField] private float springDamping = 0.7f;
    [Header("cinemachine")]
    [SerializeField] private CinemachineCamera virtualCamera;
  
    private List<GameObject> points = new List<GameObject>();
    private List<SpringJoint2D> joints = new List<SpringJoint2D>();
    private List<float> originalDistances = new List<float>();
    //[Header("White & Red BC setting")]
   private float currentRadius = 1f; 
    private float minRadius = 0.2f;
   private float maxRadius = 1.5f;
   private float sizeIncrement = 0.5f;
    private float currentScale = 0.5f;
    private float minScale = 0.2f;
    private float maxScale = 1.5f;

    void Start()
    {
        transform.position = new Vector2(transform.position.x,transform.position.y);
        SpawnCenter();
        SpawnPointsInCircle();
        ConnectPointsWithSprings();
        virtualCamera.Follow = centerPoint.transform;
    }
    void SpawnCenter()
    {
        centerPoint = Instantiate(centerPrefab, transform.position, Quaternion.identity, transform);
        Debug.Log("Center created at: " + centerPoint.transform.position);
    }
 

    void SpawnPointsInCircle()
    {
        for (int i = 0; i < porsCount; i++)
        {
            float angle = i * Mathf.PI * 2f / porsCount;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * baseRadius * currentRadius;
            Vector2 worldPos = (Vector2)transform.position + offset;
            GameObject point = Instantiate(porsPrefab, worldPos, Quaternion.identity, transform);
            points.Add(point);
        }
    }
    void ConnectPointsWithSprings()
    {
        for (int i = 0; i < porsCount; i++)
        {
            GameObject point = points[i];
            Rigidbody2D pointRb = point.GetComponent<Rigidbody2D>();

            // Connect to center
            SpringJoint2D centerJoint = point.AddComponent<SpringJoint2D>();
            centerJoint.connectedBody = centerPoint.GetComponent<Rigidbody2D>();
            centerJoint.autoConfigureDistance = false;
            float distToCenter = Vector2.Distance(point.transform.position, centerPoint.transform.position);
            centerJoint.distance = distToCenter;
            centerJoint.dampingRatio = springDamping;
            centerJoint.frequency = springFrequency;
            centerJoint.enableCollision = false;

            joints.Add(centerJoint);
            originalDistances.Add(distToCenter);

            for (int j = i + 1; j < porsCount; j++)
            {
                SpringJoint2D joint = point.AddComponent<SpringJoint2D>();
                joint.connectedBody = points[j].GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                float dist = Vector2.Distance(point.transform.position, points[j].transform.position);
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
        currentRadius += sizeChange;
        currentRadius = Mathf.Clamp(currentRadius, minRadius, maxRadius);
        currentScale = Mathf.Clamp(currentScale, minScale, maxScale);
        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].distance = originalDistances[i] * currentRadius;
        }

        float mass = currentRadius >= maxRadius ? 1.4f : 1f; foreach (var point in points)
        {
            Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
            if (rb != null) rb.mass = mass;
        }
        Debug.Log($"Scale: {currentRadius}, Mass: {mass}");
    }

    public void OnGrow(InputAction.CallbackContext context)
    {
        if (context.performed) AdjustSize(sizeIncrement);
    }

    public void OnShrink(InputAction.CallbackContext context)
    {
        if (context.performed) AdjustSize(-sizeIncrement);
    }
    void OnDrawGizmos()
    {
        if (centerPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(centerPoint.transform.position, 0.1f);
        }
    }


}


