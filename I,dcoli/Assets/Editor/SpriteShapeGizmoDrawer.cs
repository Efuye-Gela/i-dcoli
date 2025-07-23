using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

[CustomEditor(typeof(SpriteShapeController))]
public class SpriteShapeGizmoDrawer : Editor
{
    void OnSceneGUI()
    {
        SpriteShapeController controller = (SpriteShapeController)target;
        Spline spline = controller.spline;

        for (int i = 0; i < spline.GetPointCount(); i++)
        {
            Vector3 worldPos = controller.transform.TransformPoint(spline.GetPosition(i));
            Handles.color = Color.red;
            Handles.DrawSolidDisc(worldPos, Vector3.back, 0.1f);
            Handles.color = Color.white;
            Handles.Label(worldPos + Vector3.up * 0.2f, $"Point {i}");
        }
    }
}
