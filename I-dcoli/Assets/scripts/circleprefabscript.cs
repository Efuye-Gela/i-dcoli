using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PointPrefabController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Movement Settings")]
    public float moveForce = 10f;
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
        if (moveInput == Vector2.zero) return;

        Vector2 targetVelocity = moveInput * moveForce;

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
        moveInput = context.ReadValue<Vector2>();
    }
}
