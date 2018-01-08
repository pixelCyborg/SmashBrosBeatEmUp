using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DynamicTile : Tile {
    [SerializeField]
    private Sprite[] tileSprites;
    [SerializeField]
    private Sprite preview;

    private bool HasDynamic(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    /// <summary>
    /// Refreshes this tile when something changes
    /// </summary>
    /// <param name="position">The tiles position in the grid</param>
    /// <param name="tilemap">A reference to the tilemap that this tile belongs to.</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y <= 1; y++) //Runs through all the tile's neighbours 
        {
            for (int x = -1; x <= 1; x++)
            {
                //We store the position of the neighbour 
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                if (HasDynamic(tilemap, nPos)) //If the neighbour has water on it
                {
                    tilemap.RefreshTile(nPos); //Them we make sure to refresh the neighbour aswell
                }
            }
        }
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        string composition = string.Empty;

        for (int x = -1; x <= 1; x++)//Runs through all neighbours 
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0) //Makes sure that we aren't checking our self
                {
                    //If the value is a watertile
                    if (HasDynamic(tilemap, new Vector3Int(location.x + x, location.y + y, location.z)))
                    {
                        composition += 'W';
                    }
                    else
                    {
                        composition += 'E';
                    }


                }
            }
        }

        int randomVal = Random.Range(0, 100);

        if (randomVal < 15)
        {
            tileData.sprite = tileSprites[46];
        }
        else if (randomVal >= 15 && randomVal < 35)
        {

            tileData.sprite = tileSprites[48];
        }
        else
        {
            tileData.sprite = tileSprites[47];
        }



        if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[0];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = tileSprites[1];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = tileSprites[2];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = tileSprites[3];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[4];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[5];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'W' && composition[6] == 'W')
        {
            tileData.sprite = tileSprites[6];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = tileSprites[7];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W' && composition[5] == 'E' && composition[7] == 'W')
        {
            tileData.sprite = tileSprites[8];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = tileSprites[9];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'E' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = tileSprites[10];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[11];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[12];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[13];
        }
        else if (composition[0] == 'W' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = tileSprites[14];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[15];
        }
        else if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[5] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = tileSprites[16];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = tileSprites[17];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = tileSprites[18];
        }
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = tileSprites[19];
        }
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = tileSprites[20];
        }
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'W')
        {
            tileData.sprite = tileSprites[21];
        }
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
        {
            tileData.sprite = tileSprites[22];
        }
        else if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[23];
        }
        else if (composition[1] == 'W' && composition[2] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[24];
        }
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[25];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = tileSprites[26];
        }
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
        {
            tileData.sprite = tileSprites[27];
        }
        else if (composition[1] == 'E' && composition[4] == 'W' && composition[3] == 'W' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[28];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[6] == 'E' && composition[4] == 'W')
        {
            tileData.sprite = tileSprites[29];
        }
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
        {
            tileData.sprite = tileSprites[30];
        }
        else if (composition == "EWWWWEWW")
        {
            tileData.sprite = tileSprites[31];
        }
        else if (composition == "EWEWWWWE")
        {
            tileData.sprite = tileSprites[32];
        }
        else if (composition == "EWEWWWWW")
        {
            tileData.sprite = tileSprites[33];
        }
        else if (composition == "WWWWWEWW")
        {
            tileData.sprite = tileSprites[34];
        }
        else if (composition == "WWEWWWWE")
        {
            tileData.sprite = tileSprites[35];
        }
        else if (composition == "WWWWWWWE")
        {
            tileData.sprite = tileSprites[36];
        }
        else if (composition == "EWWWWWWW")
        {
            tileData.sprite = tileSprites[37];
        }
        else if (composition == "WWEWWWWW")
        {
            tileData.sprite = tileSprites[38];
        }
        else if (composition == "EWWWWWWE")
        {
            tileData.sprite = tileSprites[39];
        }
        else if (composition == "EWWWWEWE")
        {
            tileData.sprite = tileSprites[40];
        }
        else if (composition == "WWWWWEWE")
        {
            tileData.sprite = tileSprites[41];
        }
        else if (composition == "WWEWWEWW")
        {
            tileData.sprite = tileSprites[42];
        }
        else if (composition == "EWEWWEWW")
        {
            tileData.sprite = tileSprites[43];
        }
        else if (composition == "WWEWWEWE")
        {
            tileData.sprite = tileSprites[44];
        }
        else if (composition == "EWEWWEWE")
        {
            tileData.sprite = tileSprites[45];
        }

    }
#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a DynamicTile Asset
    [MenuItem("Assets/Create/DynamicTile")]
    public static void CreateDynamicTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Dynamic Tile", "Asset", "Save Dynamic Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DynamicTile>(), path);
    }
#endif
}
