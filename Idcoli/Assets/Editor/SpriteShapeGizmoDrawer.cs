using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

[CustomEditor(typeof(SpriteShapeController))]
public class SpriteShapeGizmoDrawer : Editor
{
    private static bool drawGizmos = true; // global toggle

    public override void OnInspectorGUI()
    {
        // Draw the default inspector (so you still see SpriteShapeController stuff)
        base.OnInspectorGUI();

        // Add a toggle button
        GUILayout.Space(10);
        GUI.color = drawGizmos ? Color.green : Color.red;
        if (GUILayout.Button(drawGizmos ? "Disable Gizmo Drawing" : "Enable Gizmo Drawing", GUILayout.Height(30)))
        {
            drawGizmos = !drawGizmos;
            SceneView.RepaintAll(); // refresh scene view immediately
        }
        GUI.color = Color.white;
    }

    void OnSceneGUI()
    {
        if (!drawGizmos) return;             // only draw if toggle is ON
        if (Application.isPlaying) return;   // avoid lag during play mode

        SpriteShapeController controller = (SpriteShapeController)target;
        Spline spline = controller.spline;

        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            Vector3 worldPos = controller.transform.TransformPoint(spline.GetPosition(i));
            Handles.color = Color.red;
            Handles.DrawSolidDisc(worldPos, Vector3.back, 0.1f);

            if (i%2 == 0) // only label every 5th point (to reduce lag)
            {
                Handles.color = Color.black;
                Handles.Label(worldPos + Vector3.up * 0.2f, $"Point {i}");
            }
        }
    }
}
