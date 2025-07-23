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
    public int topIndex;
    public Vector3 topOffset;

    public float moveSpeed = 2f;
    public float returnDelay = 5f;

    private Vector3[] groundOriginals;
    private Vector3[] groundTargets;
    private Vector3 topOriginal;
    private Vector3 topTarget;

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

        // Setup top point
        var splineTop = topShape.spline;
        topOriginal = splineTop.GetPosition(topIndex);
        topTarget = topOriginal + topOffset;
    }
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        yield return StartCoroutine(MoveSplinePoints(groundShape, groundIndices, groundOriginals, groundTargets));
        yield return StartCoroutine(MoveSplinePoints(topShape, new int[] { topIndex }, new Vector3[] { topOriginal }, new Vector3[] { topTarget }));

        yield return new WaitForSeconds(returnDelay);

        yield return StartCoroutine(MoveSplinePoints(groundShape, groundIndices, groundTargets, groundOriginals));
        yield return StartCoroutine(MoveSplinePoints(topShape, new int[] { topIndex }, new Vector3[] { topTarget }, new Vector3[] { topOriginal }));

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
