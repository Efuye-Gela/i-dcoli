using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelLoader : MonoBehaviour
{
    public LevelData1 levelData;
    public GameObject landPrefab, acidPrefab, hiddenPrefab, growerPrefab, shrinkerPrefab;
    public float tileSpacing = 1f;
    public float prefabScaleFactor = 0.9f;

    private void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("❌ Level data is missing.");
            return;
        }

        if (!ValidateLevelData()) return;

        LoadLevel();
        CenterCameraOnLevel();
    }

    private bool ValidateLevelData()
    {
        int expected = levelData.gridSize.x * levelData.gridSize.y;
        if (levelData.tiles == null || levelData.tiles.Length != expected)
        {
            Debug.LogError($"❌ Tile array size does not match grid size. Expected {expected}, but got {levelData.tiles?.Length ?? 0}.");
            return false;
        }
        return true;
    }

    public void LoadLevel()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        for (int y = 0; y < levelData.gridSize.y; y++)
        {
            for (int x = 0; x < levelData.gridSize.x; x++)
            {
                int index = x + y * levelData.gridSize.x;
                Vector3 localPos = new Vector3(x * tileSpacing, y * tileSpacing, 0);

                GameObject prefab = null;

                switch (levelData.tiles[index])
                {
                    case TileType.Land: prefab = landPrefab; break;
                    case TileType.Acid: prefab = acidPrefab; break;
                    case TileType.Hidden: prefab = hiddenPrefab; break;
                    case TileType.Grower: prefab = growerPrefab; break;
                    case TileType.Shrinker: prefab = shrinkerPrefab; break;
                }

                if (prefab != null)
                {
                    GameObject tile = Instantiate(prefab, localPos, Quaternion.identity, transform);
                    tile.transform.localScale = Vector3.one * tileSpacing * prefabScaleFactor;
                }
            }
        }
    }

    private void CenterCameraOnLevel()
    {
        if (Camera.main == null)
        {
            Debug.LogWarning("⚠️ No Main Camera found!");
            return;
        }

        Vector2 levelCenter = new Vector2(
            (levelData.gridSize.x - 1) * tileSpacing / 2f,
            (levelData.gridSize.y - 1) * tileSpacing / 2f
        );

        Camera.main.transform.position = new Vector3(levelCenter.x, levelCenter.y, -10f);
    }

    private void OnDrawGizmos()
    {
        if (levelData == null || levelData.tiles == null) return;

        Gizmos.color = Color.gray;

        // Offset gizmo grid so it appears centered in Scene View before play
        Vector3 offset = Application.isPlaying ? Vector3.zero : GetEditorOffset();

        int labelNumber = 1;

        for (int y = 0; y < levelData.gridSize.y; y++)
        {
            for (int x = 0; x < levelData.gridSize.x; x++)
            {
                Vector3 pos = new Vector3(x * tileSpacing, y * tileSpacing, 0) + offset;
                Gizmos.DrawWireCube(pos, Vector3.one * tileSpacing * 0.9f);

#if UNITY_EDITOR
                GUIStyle style = new GUIStyle
                {
                    normal = new GUIStyleState { textColor = Color.white },
                    fontSize = 12,
                    alignment = TextAnchor.MiddleCenter
                };

                // Numbering from bottom-left upward
                Handles.Label(pos + new Vector3(0, 0, -0.1f), labelNumber.ToString(), style);
                labelNumber++;
#endif
            }
        }
    }

    private Vector3 GetEditorOffset()
    {
        if (levelData == null) return Vector3.zero;

        Vector2 levelSize = new Vector2(
            levelData.gridSize.x * tileSpacing,
            levelData.gridSize.y * tileSpacing
        );

        return new Vector3(levelSize.x, levelSize.y, 0) * -0.5f + new Vector3(tileSpacing / 2f, tileSpacing / 2f, 0);
    }
}
