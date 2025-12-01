using System.Collections.Generic;
using UnityEngine;

public class JoystickMove : MonoBehaviour
{
    public Joystick movementJoystick;
    public float playerSpeed;

    private Bacteria currentBacteria;
    private List<Rigidbody2D> porsRBs;

    private void Start()
    {
        FindAndSetupBacteria();
    }

    private void FindAndSetupBacteria()
    {
        currentBacteria = FindAnyObjectByType<Bacteria>();
        if (currentBacteria != null)
        {
            porsRBs = currentBacteria.GetPorsRigidbodies();
        }
    }

    private void FixedUpdate()
    {
        // If we lost our reference, try to find it again
        if (porsRBs == null || porsRBs.Count == 0 || porsRBs[0] == null)
        {
            FindAndSetupBacteria();
            return;
        }

        Vector2 move = movementJoystick.Direction * playerSpeed;

        foreach (Rigidbody2D rb in porsRBs)
        {
            if (rb != null)
            {
                rb.linearVelocity = move;
            }
        }
    }
}