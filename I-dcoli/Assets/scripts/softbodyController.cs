using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SoftBodyController : MonoBehaviour
{
    [Header("Soft Body Settings")]
    public GameObject pointPrefab;
    public int pointCount = 10;
    private float radius = 2.5f;
    public float springFrequency = 5f;
    public float springDamping = 0.7f;
    public float moveForce = 10f;
    private bool isshrinked = false;
    private List<GameObject> points = new List<GameObject>();
    private List<SpringJoint2D> joints = new List<SpringJoint2D>();
    private List<float> originalDistances = new List<float>(); 

    void Start()
    {
        transform.position = new Vector2(-35.4f, 18.6f);
        SpawnPointsInCircle();
        ConnectPointsWithSprings();
    }

    void Update()
    {
        HandlePlayerInput();
       
    }

    void HandlePlayerInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input != Vector2.zero)
        {
            foreach (GameObject point in points)
            {
                Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
                rb.AddForce(input.normalized * moveForce, ForceMode2D.Force);
            }
        }
       
    }

    void SpawnPointsInCircle()
    {
        for (int i = 0; i < pointCount; i++)
        {
            float angle = i * Mathf.PI * 2f / pointCount;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
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
                var joint = points[i].AddComponent<SpringJoint2D>();
                joint.connectedBody = points[j].GetComponent<Rigidbody2D>();
                joint.autoConfigureDistance = false;
                joint.distance = Vector2.Distance(points[i].transform.position, points[j].transform.position);
                joint.dampingRatio = springDamping;
                joint.frequency = springFrequency;
                joint.enableCollision = false;
                float dist = Vector2.Distance(points[i].transform.position, points[j].transform.position);

                joints.Add(joint); 
                originalDistances.Add(dist);
            }
        }
    }
    public enum JointSizeState { Small, Original, Large }

    private JointSizeState currentState = JointSizeState.Original;

    public void SetJointState(JointSizeState newState)
    {
        float targetScale = 1f;
        float targetMass = 1f;

        switch (newState)
        {
            case JointSizeState.Small:
                targetScale = 0.5f;
                targetMass = 0.5f;
                break;
            case JointSizeState.Original:
                targetScale = 1f;
                targetMass = 1f;
                break;
            case JointSizeState.Large:
                targetScale = 2f;
                targetMass = 2f;
                break;
        }

        // Update joint distances
        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].distance = originalDistances[i] * targetScale;
        }

        // ✅ Set mass on all point Rigidbodies
        foreach (var point in points)
        {
            Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.mass = targetMass;
            }
            else
            {
                Debug.LogWarning("Missing Rigidbody2D on one of the soft body points.");
            }
        }

        currentState = newState;
    }




}
