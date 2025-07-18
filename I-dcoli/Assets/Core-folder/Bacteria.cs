using UnityEngine;
 using System.Collections.Generic;
using Unity.Cinemachine;
public class Bacteria : MonoBehaviour
{
    [Header("Game_Objects")]
    [SerializeField] private GameObject porsPrefab;
    [SerializeField] private GameObject centerPrefab;
    private GameObject centerPoint;
    [Header("Properties")]
    [SerializeField] private int porsCount = 10;
    [SerializeField] private float radiusInBetweenPors = 1f;
    [SerializeField] private float springFrequency = 5f;
    [SerializeField] private float springDamping = 0.7f;

    private float currentScale = 1f;
    private List<GameObject> points = new List<GameObject>();
    private List<SpringJoint2D> joints = new List<SpringJoint2D>();
    private List<float> originalDistances = new List<float>();
    void Start()
    {
        transform.position = new Vector2(transform.position.x,transform.position.y);
        SpawnCenter();
        SpawnPointsInCircle();
        ConnectPointsWithSprings();

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
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radiusInBetweenPors * currentScale;
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
  




}
