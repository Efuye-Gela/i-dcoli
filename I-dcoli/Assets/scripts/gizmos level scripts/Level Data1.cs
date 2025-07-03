using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data")]
public class LevelData1 : ScriptableObject
{
    public Vector2Int gridSize;
    public TileType[] tiles;

#if UNITY_EDITOR
    private void OnValidate()
    {
        int expected = gridSize.x * gridSize.y;

        if (expected <= 0) return;

        if (tiles == null || tiles.Length != expected)
        {
            Debug.LogWarning($"Resizing tile array to match grid size: {gridSize.x} x {gridSize.y} = {expected}");
            System.Array.Resize(ref tiles, expected);
        }
    }
#endif
}

public enum TileType
{
    Empty,
    Land,
    Acid,
    Hidden,
    Grower,
    Shrinker
}
