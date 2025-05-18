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

    private List<GameObject> points = new List<GameObject>();
    private List<SpringJoint2D> joints = new List<SpringJoint2D>();
    private List<float> originalDistances = new List<float>(); 

    void Start()
    {
        transform.position = new Vector2(-37.5f, 13.9f);
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

    public void ExpandJoints(float multiplier = 2.5f)
    {
        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].distance = originalDistances[i] * multiplier;
        }
    }

    public void shrinkJoints(float shrinker = 2f)
    {
        

      
        for (int i = 0; i < joints.Count; i++)
        {
            joints[i].distance = originalDistances[i] / shrinker;
        }

    }


}
