using System.Collections;
using Unity.Cinemachine; 
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    [Header("Gate Shapes")]
    public SpriteShapeController groundShape;
    public int[] groundIndices;
    public Vector3[] groundOffsets;

    public SpriteShapeController topShape;
    public int[] topIndices;
    public Vector3[] topOffsets;

    public SpriteShapeController triggerShape;
    public int[] triggerIndices;
    public Vector3[] triggerOffsets;

    [Header("Cinemachine & Timeline")]
    public bool playCutsceneBeforeGate = false;
    public PlayableDirector timelineDirector;
    public CinemachineCamera gameplayVCam; 
    public CinemachineCamera cutsceneVCam; 

    public float moveSpeed = 2f;
    public float returnDelay = 5f;

    private Vector3[] groundOriginals;
    private Vector3[] groundTargets;
    private Vector3[] topOriginals;
    private Vector3[] topTargets;
    private Vector3[] triggerOriginals;
    private Vector3[] triggerTargets;
    private bool isAnimating = false;

    // Cinemachine camera priorities
    private int gameplayCameraPriority;
    private int cutsceneCameraPriority;

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

        // Setup trigger points
        if (triggerShape != null && triggerIndices != null && triggerOffsets != null)
        {
            var splineTrigger = triggerShape.spline;
            triggerOriginals = new Vector3[triggerIndices.Length];
            triggerTargets = new Vector3[triggerIndices.Length];
            for (int i = 0; i < triggerIndices.Length; i++)
            {
                triggerOriginals[i] = splineTrigger.GetPosition(triggerIndices[i]);
                triggerTargets[i] = triggerOriginals[i] + triggerOffsets[i];
            }
        }

        // Store original camera priorities
        if (gameplayVCam != null)
        {
            gameplayCameraPriority = gameplayVCam.Priority;
        }
        if (cutsceneVCam != null)
        {
            cutsceneCameraPriority = cutsceneVCam.Priority;
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

        // Play Timeline cutscene if configured
        if (playCutsceneBeforeGate && timelineDirector != null)
        {
            Debug.Log("Starting Timeline cutscene...");
            yield return StartCoroutine(PlayTimelineCutscene());
            Debug.Log("Timeline cutscene completed, starting gate movement...");
        }

        // Move ground up
        yield return StartCoroutine(MoveSplinePoints(groundShape, groundIndices, groundOriginals, groundTargets));

        // Move top up
        yield return StartCoroutine(MoveSplinePoints(topShape, topIndices, topOriginals, topTargets));

        // Move trigger if configured
        if (triggerShape != null && triggerIndices != null && triggerIndices.Length > 0)
        {
            yield return StartCoroutine(MoveSplinePoints(triggerShape, triggerIndices, triggerOriginals, triggerTargets));
        }

        yield return new WaitForSeconds(returnDelay);

        // Return ground
        yield return StartCoroutine(MoveSplinePoints(groundShape, groundIndices, groundTargets, groundOriginals));

        // Return top
        yield return StartCoroutine(MoveSplinePoints(topShape, topIndices, topTargets, topOriginals));

        // Return trigger if configured
        if (triggerShape != null && triggerIndices != null && triggerIndices.Length > 0)
        {
            yield return StartCoroutine(MoveSplinePoints(triggerShape, triggerIndices, triggerTargets, triggerOriginals));
        }

        isAnimating = false;
    }

    IEnumerator PlayTimelineCutscene()
    {
        // Switch to cutscene camera if available
        if (cutsceneVCam != null)
        {
            cutsceneVCam.Priority = gameplayCameraPriority + 10; // Higher priority
        }
        else if (gameplayVCam != null)
        {
            // If no dedicated cutscene camera, at least ensure gameplay camera is active
            gameplayVCam.Priority = gameplayCameraPriority + 5;
        }

        // Play the timeline
        timelineDirector.Play();

        // Wait for timeline to complete
        while (timelineDirector.state == PlayState.Playing)
        {
            yield return null;
        }

        // Restore camera priorities
        if (cutsceneVCam != null)
        {
            cutsceneVCam.Priority = cutsceneCameraPriority;
        }
        if (gameplayVCam != null)
        {
            gameplayVCam.Priority = gameplayCameraPriority;
        }
    }

    IEnumerator MoveSplinePoints(SpriteShapeController shape, int[] indices, Vector3[] from, Vector3[] to)
    {
        if (shape == null || indices == null || indices.Length == 0)
            yield break;

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

    // Method to open gate with timeline cutscene
    public void OpenGateWithTimelineCutscene()
    {
        if (!isAnimating)
        {
            playCutsceneBeforeGate = true;
            StartCoroutine(OpenAndClose());
        }
    }

    // Method to open gate without cutscene
    public void OpenGateWithoutCutscene()
    {
        if (!isAnimating)
        {
            playCutsceneBeforeGate = false;
            StartCoroutine(OpenAndClose());
        }
    }

    // Called when timeline ends (can be connected via timeline signal receiver)
    public void OnTimelineComplete()
    {
        Debug.Log("Timeline cutscene completed");
    }
}