using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public float playerSpeed;

    private List<Rigidbody2D> porsRBs;

    private void Start()
    {
        Bacteria bacteria = FindAnyObjectByType<Bacteria>();
        porsRBs = bacteria.GetPorsRigidbodies();
    }

    private void FixedUpdate()
    {
        Vector2 move = movementJoystick.Direction * playerSpeed;

        foreach (Rigidbody2D rb in porsRBs)
        {
            rb.linearVelocity = move;
        }
    }
}
