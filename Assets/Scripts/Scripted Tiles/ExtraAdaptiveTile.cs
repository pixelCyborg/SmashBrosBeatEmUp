using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExtraAdaptiveTile : Tile {
    public Sprite m_Preview;

    //No Exposed Sides
    public Sprite underground;
    //One Exposed Side
    public Sprite surface_n;
    public Sprite surface_e;
    public Sprite surface_s;
    public Sprite surface_w;
    //Two Exposed Sides
    public Sprite corner_ne;
    public Sprite corner_se;
    public Sprite corner_sw;
    public Sprite corner_nw;
    //Three Exposed Sides
    public Sprite peninsula_n;
    public Sprite peninsula_e;
    public Sprite peninsula_s;
    public Sprite peninsula_w;
    //All Exposed Sides
    public Sprite standalone;

    // This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasRoadTile(tilemap, position))
                    tilemap.RefreshTile(position);
            }
    }
    // This determines which sprite is used based on the RoadTiles that are adjacent to it and rotates it to fit the other tiles.
    // As the rotation is determined by the RoadTile, the TileFlags.OverrideTransform is set for the tile.
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int mask = HasRoadTile(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
        mask += HasRoadTile(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
        mask += HasRoadTile(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
        mask += HasRoadTile(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;

        Sprite sprite = GetSprite((byte)mask);
        if (sprite != null)
        {
            tileData.sprite = sprite;
            tileData.color = Color.white;
            //tileData.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity , Vector3.one);
            tileData.flags = TileFlags.LockTransform;
            tileData.colliderType = ColliderType.Grid;
        }
        else
        {
            Debug.LogWarning("Not enough sprites in RoadTile instance");
        }
    }
    // This determines if the Tile at the position is the same RoadTile.
    private bool HasRoadTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    private Sprite GetSprite(byte mask)
    {
        switch (mask)
        {
            case 0: return standalone;

            case 3: return corner_sw;
            case 6: return corner_nw;
            case 9: return corner_se;
            case 12: return corner_ne;

            case 1: return peninsula_s;
            case 2: return peninsula_w;
            case 4: return peninsula_n;
            case 5: return peninsula_n;
            case 10: return peninsula_w;
            case 8: return peninsula_e;

            case 7: return surface_w;
            case 11: return surface_s;
            case 13: return surface_e;
            case 14: return surface_n;

            case 15: return underground;
        }
        return null;
    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a DynamicTile Asset
    [MenuItem("Assets/Create/ExtraAdaptiveTile")]
    public static void CreateExtraAdaptiveTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Adaptive Tile", "New Adaptive Tile", "Asset", "Save Adaptive Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ExtraAdaptiveTile>(), path);
    }
#endif
}
