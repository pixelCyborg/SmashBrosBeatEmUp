using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Destructibles : MonoBehaviour {
    Tilemap tilemap;
    public TileBase debugTile;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void DestroyTile(Vector3 worldPosition)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPosition);
        Debug.Log("World Position: " + worldPosition + " | Cell Position: " + cellPos);
        tilemap.SetTile(cellPos, null);
        tilemap.RefreshAllTiles();
    }
}
