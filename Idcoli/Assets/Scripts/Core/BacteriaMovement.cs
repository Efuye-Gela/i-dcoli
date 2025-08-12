using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class BacteriaMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed = 10f;
    private Vector2 inputPosition;
    public float smoothTime = 0.1f;
    public bool applySmoothing = true;
    private Vector2 velocitySmoothing;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!rb)
        {
            Debug.LogError("Missing Rigidbody2D on: " + name);
            enabled = false;
            return;
        }

        rb.gravityScale = 1f;
        rb.linearDamping = 1.5f;
    }


    private void FixedUpdate()
    {
        if (inputPosition == Vector2.zero) return;

        Vector2 targetVelocity = inputPosition * moveSpeed;

        if (applySmoothing)
        {
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocitySmoothing, smoothTime);
        }
        else
        {
            rb.linearVelocity = targetVelocity;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputPosition = context.ReadValue<Vector2>();
    }

}
