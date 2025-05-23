using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class wallMovementScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject wall1;
    [SerializeField] private GameObject wall2;
    [SerializeField] private GameObject wallMover;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveoutspeed = 10f;
    [SerializeField] private float requiredMass = 2f;
    [SerializeField] private float moveBackDelay = 1f;


    private bool triggered = false;
    private bool moving = false;

    private Vector3 wall1TargetPos;
    private Vector3 wall2TargetPos;
    private Vector3 wallMoverTargetPos;
    private Vector3 origionalPosWall1;
    private Vector3 origionalPosWall2;
    private Vector3 origionalPosWallMover;
    private bool ismoving = false;

    private void Start()
    {
        wall1TargetPos = wall1.transform.position;
        wall2TargetPos = wall2.transform.position;
        wallMoverTargetPos = wallMover.transform.position;


        origionalPosWall1 = wall1.transform.position;
        origionalPosWall2 = wall2.transform.position;
        origionalPosWallMover = wallMover.transform.position;

        wall1TargetPos = wall1.transform.position;
        wall2TargetPos = wall2.transform.position;
        wallMoverTargetPos = wallMover.transform.position;
    }

    private void Update()
    {
        
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (moving)
        {
            
                wall1.transform.position = Vector3.MoveTowards(wall1.transform.position, wall1TargetPos, moveSpeed * Time.deltaTime);
                wall2.transform.position = Vector3.MoveTowards(wall2.transform.position, wall2TargetPos, moveSpeed * Time.deltaTime);
                wallMover.transform.position = Vector3.MoveTowards(wallMover.transform.position, wallMoverTargetPos, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(wall1.transform.position, wall1TargetPos) < 0.01f &&
                    Vector3.Distance(wall2.transform.position, wall2TargetPos) < 0.01f &&
                    Vector3.Distance(wallMover.transform.position, wallMoverTargetPos) < 0.01f)
                {
                    moving = false;
                    StartCoroutine(MoveBackAfterDelay());
                    Debug.Log("All walls moved to target positions.");
                }
            
        }
        if (ismoving )
        {
            wall1.transform.position = Vector3.MoveTowards(
                wall1.transform.position,
                new Vector3(
                    wall1.transform.position.x,
                    origionalPosWall1.y,
                    origionalPosWall1.z
                ),
                moveoutspeed * Time.deltaTime
            );

            wall2.transform.position = Vector3.MoveTowards(
                wall2.transform.position,
                new Vector3(
                    wall2.transform.position.x,
                    origionalPosWall2.y,
                    origionalPosWall2.z
                ),
                moveoutspeed * Time.deltaTime
            );

            wallMover.transform.position = Vector3.MoveTowards(
                wallMover.transform.position,
                new Vector3(
                    wallMover.transform.position.x,
                    origionalPosWallMover.y,
                    origionalPosWallMover.z
                ),
                moveoutspeed * Time.deltaTime
            );

            if (Mathf.Abs(wall1.transform.position.y - origionalPosWall1.y) < 0.01f &&
                Mathf.Abs(wall2.transform.position.y - origionalPosWall2.y) < 0.01f &&
                Mathf.Abs(wallMover.transform.position.y - origionalPosWallMover.y) < 0.01f)
            {
                ismoving = false;
                Debug.Log("The walls are back to original positions.");
                triggered = false;
            }
        }



    }
    private System.Collections.IEnumerator MoveBackAfterDelay()
    {
        yield return new WaitForSeconds(moveBackDelay);
        ismoving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.transform.root.CompareTag("MainPlayer"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("Detected player with mass: " + rb.mass);
            }

            if (rb != null && Mathf.Approximately(rb.mass, requiredMass))
            {
                Debug.Log("Wall trigger activated.");
             
                wall1TargetPos = new Vector3(wall1.transform.transform.position.x, 9.18f, wall1.transform.position.z);
                wall2TargetPos = new Vector3(wall2.transform.transform.position.x, -18.8f, wall2.transform.position.z);
                wallMoverTargetPos = new Vector3(wallMover.transform.transform.position.x, -12.62f, wallMover.transform.position.z);

                moving = true;
                triggered = true;
            }
            else
            {
                Debug.Log("Incorrect mass or Rigidbody missing.");
            }
        }
        else
        {
            Debug.Log("Triggered by non-player object: " + collision.name);
        }
    }
}
