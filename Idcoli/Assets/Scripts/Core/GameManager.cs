using UnityEngine;
using UnityEngine.U2D;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Ground Shape")]
    public SpriteShapeController groundShape;
    public int[] groundIndices;
    public Vector3[] groundOffsets;

    [Header("Top Shape")]
    public SpriteShapeController topShape;
    public int[] topIndices;
    public Vector3[] topOffsets;

    public float moveSpeed = 2f;
    public float returnDelay = 5f;

    private Vector3[] groundOriginals;
    private Vector3[] groundTargets;

    private Vector3[] topOriginals;
    private Vector3[] topTargets;

    private bool isAnimating = false;

    void Start()
    {
        // Setup ground points
        var splineGround = groundShape.spline;
        groundOriginals = new Vector3[groundIndices.Length];
        groundTargets = new Vector3[groundIndices.Length];
        for (int i = 0; i < groundIndices.Length; i++)
        {
            groundOriginals[i] = splineGround.GetPosition(groundIndices[i]);
            groundTargets[i] = groundOriginals[i] + groundOffsets[i];
        }

        // Setup top points
        var splineTop = topShape.spline;
        topOriginals = new Vector3[topIndices.Length];
        topTargets = new Vector3[topIndices.Length];
        for (int i = 0; i < topIndices.Length; i++)
        {
            topOriginals[i] = splineTop.GetPosition(topIndices[i]);
            topTargets[i] = topOriginals[i] + topOffsets[i];
        }
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            GameManager1 gm = FindAnyObjectByType<GameManager1>();
            if (gm != null)
            {
                gm.RestartGame();
            }
            else
            {
                Debug.LogWarning("GameManager1 not found in the scene.");
            }
        }
    }

    public void OpenGate()
    {
        if (!isAnimating)
            StartCoroutine(OpenAndClose());
    }

    IEnumerator OpenAndClose()
    {
        isAnimating = true;

        // Move ground up
        yield return StartCoroutine(MoveSplinePoints(groundShape, groundIndices, groundOriginals, groundTargets));

        // Move top up
        yield return StartCoroutine(MoveSplinePoints(topShape, topIndices, topOriginals, topTargets));

        yield return new WaitForSeconds(returnDelay);

        // Return ground
        yield return StartCoroutine(MoveSplinePoints(groundShape, groundIndices, groundTargets, groundOriginals));

        // Return top
        yield return StartCoroutine(MoveSplinePoints(topShape, topIndices, topTargets, topOriginals));

        isAnimating = false;
    }

    IEnumerator MoveSplinePoints(SpriteShapeController shape, int[] indices, Vector3[] from, Vector3[] to)
    {
        Spline spline = shape.spline;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;

            for (int i = 0; i < indices.Length; i++)
            {
                Vector3 pos = Vector3.Lerp(from[i], to[i], t);
                spline.SetPosition(indices[i], pos);
            }

            shape.BakeMesh();
            yield return null;
        }
    }
}
