using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapGenerator : MonoBehaviour {

    /// <summary>
    /// Tile Values
    /// 
    ///     0 ==> Empty
    ///     1 ==> Ground
    ///     2 ==> Platform
    ///     3 ==> Ladder
    ///     4 ==> Ladder Through Ground
    /// 
    /// </summary>

    [Header("Tilemaps")]
    public Tilemap targetMap;
    public Tilemap ladderMap;
    public Tilemap platformMap;
    public Tilemap backgroundMap;

    [Header("Tile Prefabs")]
    public Tile fillTile;
    public Tile emptyTile;
    public Tile ladderTile;
    public Tile platformTile;
    
    [Header ("Room Colliders")]
    public Transform roomParent;
    public GameObject roomPrefab;

    [Header("Interactables")]
    public GameObject doorObject;
    public Transform interactableParent;

    [Header("Enemies")]
    public Transform enemyParent;
    public GameObject skeletonEnemy;
    public GameObject floaterEnemy;

    int[,] map;
    List<Ladder> ladders;
    List<Platform> platforms;

    [Header("Map Generation Settings")]
    public int width = 32;
    public int height = 32;
    public string seed;
    public bool useRandomSeed;
    private Room[] rooms;
    public bool flattenRooms;
    private Coord playerStart;

    private const int MAX_JUMPABLE_HEIGHT = 3;
    private const int MAX_JUMPABLE_GAP = 8;

    [Range(0, 100)]
    public int randomFillPercent = 40;
    [Range(0, 10)]
    public int smoothIterations = 5;
    [Range(0, 200)]
    public int wallThreshold = 50;
    [Range(0, 200)]
    public int roomThreshold = 50;
    [Range(1, 5)]
    public int passageWidth = 1;
    [Range(1, 10)]
    public int wallHeight = 5;

    [Header("Ladder Parameters")]
    [Range(1, 20)]
    public int minLadderRange = 3;
    [Range(5, 50)]
    public int maxLadderRange = 25;
    [Range(1, 50)]
    public int ladderGroundThreshold = 20;

    [Header("Platform Parameters")]
    [Range(1, 10)]
    public int minPlatformLength = 5;
    [Range(1, 30)]
    public int maxPlatformLength = 15;
    [Range(0.0f, 1.0f)]
    public float groundProximityRatio = 0.33f;

    [Range(0.0f, 100.0f)]
    public float enemySpawnRate = 20.0f;

    struct Coord : IEquatable<Coord>, IComparable<Coord>
    {
        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Coord other)
        {
            return this.x == other.x && this.y == other.y;
        }

        public int CompareTo(Coord other)
        {
            return x.CompareTo(other.x);
        }
    }

    struct Ladder : IComparable<Ladder>
    {
        public Coord[] tiles;
        public int burrowCount;

        public Ladder(Coord[] tiles, int burrowCount)
        {
            this.tiles = tiles;
            this.burrowCount = burrowCount;
        }

        //Sort ladders from left to right
        public int CompareTo(Ladder other)
        {
            return tiles[0].y.CompareTo(other.tiles[0].y);
        }
    }

    struct Platform : IComparable<Platform>
    {
        public Coord[] tiles;

        public Platform(Coord[] tiles)
        {
            this.tiles = tiles;
        }

        //Sort ladders from left to right
        public int CompareTo(Platform other)
        {
            return tiles[0].x.CompareTo(other.tiles[0].x);
        }
    }



    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothIterations; i++)
        {
            SmoothMap();
        }

        ProcessMap();
        FindLadders();
        FindPlatforms();
        PlaceEnemies();

        PaintMapTiles();

        PlacePlayerAtStart();
        PlaceDoorAtExit();
    }

    //Map Flags =========================
    Coord BestStartPoint()
    {
        Coord startPoint;
        int y = height - 1;
        bool foundStartPoint = false;
        List<Coord> possibleStarts = new List<Coord>();

        //Find a space close to the top that
        while (!foundStartPoint)
        {
            y--;
            for (int x = 0; x < width - 1; x++)
            {
                if (map[x, y] == 0)
                {
                    if (HasRoom(x, y))
                    {
                        possibleStarts.Add(new Coord(x, y));
                        foundStartPoint = true;
                    }
                }
            }
        }

        startPoint = possibleStarts[UnityEngine.Random.Range(0, possibleStarts.Count - 1)];

        //From that space draw downward until hitting ground
        bool hitGround = false;
        y = startPoint.y;
        while(!hitGround)
        {
            if(map[startPoint.x, y] == 1)
            {
                hitGround = true;
                startPoint.y = y + 2;
            }
            y--;
        }

        return startPoint;
    }

    //Map Flags =========================
    Coord BestExitPoint()
    {
        Coord endPoint;
        int y = 1;
        bool foundEndPoint = false;
        List<Coord> possibleExits = new List<Coord>();

        //Find a space close to the top that
        while (!foundEndPoint)
        {
            y++;
            for (int x = 0; x < width - 1; x++)
            {
                if (map[x, y] == 0)
                {
                    if (HasRoom(x, y))
                    {
                        possibleExits.Add(new Coord(x, y));
                        foundEndPoint = true;
                    }
                }
            }
        }

        endPoint = possibleExits[UnityEngine.Random.Range(0, possibleExits.Count - 1)];

        //From that space draw downward until hitting ground
        bool hitGround = false;
        y = endPoint.y;
        while (!hitGround)
        {
            if (map[endPoint.x, y] == 1)
            {
                hitGround = true;
                endPoint.y = y + 2;
            }
            y--;
        }

        return endPoint;
    }

    bool IsOutOfBounds(int x, int y)
    {
        return x < 0 || x > width - 1 || y < 0 || y > height - 1;
    }

    bool NoNeighboringTiles (int x, int y)
    {
        //Check left/right
        return map[x, y] != 1 && map[x - 1, y] != 1 && map[x + 1, y] != 1 
        //Up.down
        && map[x, y + 1] != 1 && map[x, y - 1] != 1
        //Check corners
        && map[x - 1, y + 1] != 1 && map[x - 1, y - 1] != 1
        && map[x + 1, y + 1] != 1 && map[x + 1, y - 1] != 1
        //Check for enemies
        && Physics2D.OverlapCircle(CoordToWorldPoint(new Coord(x, y)), 3.0f, 1 << LayerMask.NameToLayer("Enemy")) == null;

    }

    bool IsFloorTile(int x, int y)
    {
        try
        {
            return map[x, y] != 0 && map[x, y + 1] == 0 && map[x, y + 2] == 0;
        }
        catch(Exception e)
        {
            return false;
        }
    }

    /// Utility Methods
    bool HasRoom(int centerX, int centerY)
    {
        for(int x = centerX - 1; x <= centerX + 1; x++)
        {
            for (int y = centerY + 1; y >= centerY - 1; y--)
            {
                if(map[x,y] != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    

    void ClearSquare(Coord start, Coord finish)
    {
        if(finish.x < start.x)
        {
            int tempX = start.x;
            start.x = finish.x;
            finish.x = tempX;
        }

        if (finish.y < start.y)
        {
            int tempY = start.y;
            start.y = finish.y;
            finish.y = tempY;
        }

        for (int x = start.x; x < finish.x; x++) 
        {
            for(int y = start.y; y < finish.y; y++)
            {
                map[x, y] = 0;
            }
        }
    }

    //Search for ladder

    //Checks to see if there is a wall 4 or more tiles high on the left or right of this tile
    //If there is no wall, return Vector2.zero
    //Otherwise, return the position of the tile above the wall relative to the tested tile
    private Vector2 CheckWallHeight(int x, int y) {
        //If there is a ground tile on either side, iterate upward until hitting a non-ground tile
        //If we have iterated more than 4 times, there is a wall

        //Check Left
        if (!IsOutOfBounds(x - 1, y) && map[x - 1, y] == 1)
        {
            int index = 0;
            while(map[x - 1, y + index] == 1)
            {
                index++;
            }

            if (index > MAX_JUMPABLE_HEIGHT) return new Vector2(x - 1, y + index);
        }

        //Check Right
        if (!IsOutOfBounds(x + 1, y) && map[x + 1, y] == 1)
        {
            int index = 0;
            while (map[x + 1, y + index] == 1)
            {
                index++;
            }

            if (index > MAX_JUMPABLE_HEIGHT) return new Vector2(x + 1, y + index);
        } 

        //If none of these are the case, either the wall is not tall enough or there is none. Either way return "false"
        return Vector2.zero;
    }

    //If there is a chunk of ground that has empty space below it and above it, check to see if there should be a ladder there
    //Measure from the ABOVE tile
    private void MeasureLadder(int x, int y)
    {
        //Make sure the tile above this is open
        if (IsOutOfBounds(x, y + 1) || map[x, y + 1] == 1) return;

        List<Coord> ladderCoords = new List<Coord>();
        ladderCoords.Add(new Coord(x, y));

        int index = 1;
        int burrowingDepth = 0;

        while(!IsFloorTile(x, y - index))
        {
            if (IsOutOfBounds(x, y - index) || index > maxLadderRange) return;

            ladderCoords.Add(new Coord(x, y - index));
            if (map[x, y - index] == 1)
            {
                burrowingDepth++;
            }

            index++;
        }

        //If all conditions are satisfied, mark the coordinates to place ladders
        if(index > minLadderRange && burrowingDepth < ladderGroundThreshold)
        {
            ladders.Add(new Ladder(ladderCoords.ToArray(), burrowingDepth));
        }
    }

    //Build the platforms, simply check if an empty space is between the min and max platform length
    private void BuildPlatform(int x, int y)
    {
        List<Coord> tiles = new List<Coord>();
        int index = 1;
        int groundProximity = 0;

        while(map[x + index,y] == 0)
        {
            if (IsOutOfBounds(x + index, y) || index > maxPlatformLength) return;

            if (index > 0 && CloseToGround(x + index, y))
            {
                groundProximity++;
            }

            tiles.Add(new Coord(x + index, y));

            index++;
        }

        if(tiles.Count >= minPlatformLength && groundProximity < tiles.Count * groundProximityRatio)
        {
            platforms.Add(new Platform(tiles.ToArray()));
            foreach (Coord coord in tiles)
            {
                map[coord.x, coord.y] = 2;
            }
        }
    }

    private bool CloseToGround(int x, int y)
    {
        bool closeToGround = false;

        for(int i = 0; i < MAX_JUMPABLE_HEIGHT; i++)
        {
            if (IsOutOfBounds(x, y - i) || map[x, y - i] != 0) closeToGround = true;
        }

        for(int i = 1; i <= 2; i++)
        {
            if(IsOutOfBounds(x, y + i) || map[x, y + i] != 0) closeToGround = true;
        }

        return closeToGround;
    }

    //Map Structure ============================================================
    void PlacePlayerAtStart()
    {
        Coord startPoint = BestStartPoint();
        playerStart = startPoint;
        Player player = FindObjectOfType<Player>();
        if (player == null) return;

        player.transform.position = CoordToWorldPoint(startPoint);
    }

    void PlaceDoorAtExit()
    {
        for(int i = interactableParent.childCount - 1; i >= 0; i--)
        {
            Destroy(interactableParent.GetChild(i).gameObject);
        }

        Coord exitPoint = BestExitPoint();
        GameObject door = (GameObject)Instantiate(doorObject, CoordToWorldPoint(exitPoint) - Vector3.up * 0.6f, Quaternion.identity, interactableParent);
        door.GetComponent<Door>().targetScene = "Camp";
    }

    private void FindLadders()
    {
        ladders = new List<Ladder>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(IsFloorTile(x, y)) MeasureLadder(x, y);
            }
        }

        ladders.Sort();
        CombineLadders();
    }

    private void FindPlatforms()
    {
        platforms = new List<Platform> ();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1 && 
                    x + 1 < width && map[x + 1,y] == 0 && 
                    x - 1 > 0 && map[x - 1, y] == 1) BuildPlatform(x, y);
            }
        }
    }

    private void CombineLadders()
    {
        List<Ladder> survivingLadders = new List<Ladder>();
        List<Ladder> ladderGroup = new List<Ladder>();

        foreach (Ladder ladder in ladders)
        {
            //If the ladders are within the same vicinity, add them to the current ladder group (or if the ladder group is currently empty)
            if (ladderGroup.Count == 0 ||
                (Mathf.Abs(ladder.tiles[0].x - ladderGroup[ladderGroup.Count - 1].tiles[0].x) < 3
                && Mathf.Abs(ladder.tiles[0].y - ladderGroup[ladderGroup.Count - 1].tiles[0].y) < 5))
            {
                ladderGroup.Add(ladder);
            }
            //Otherwise, we should find the shortest ladder and start a new ladder group
            else
            {
                ladderGroup.Sort(delegate (Ladder a, Ladder b)
                {
                    return a.tiles.Length.CompareTo(b.tiles.Length);
                });

                survivingLadders.Add(ladderGroup[0]);
                ladderGroup = new List<Ladder>();
            }
        }

        ladders = survivingLadders;
    }

    void PlaceEnemies()
    {
        for(int i = enemyParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(enemyParent.GetChild(i).gameObject);
        }

        for(int i = 0; i < platforms.Count; i++)
        {
            PlaceEnemiesOnPlatform(platforms[i]);
        }

        for(int i = 0; i < rooms.Length; i++)
        {
            PlaceEnemiesInRoom(rooms[i]);
        }
    }

    void PlaceEnemiesOnPlatform(Platform platform)
    {
        foreach (Coord coord in platform.tiles)
        {
            if (IsFloorTile(coord.x, coord.y) && IsFloorTile(coord.x - 1, coord.y) && IsFloorTile(coord.x + 1, coord.y))
            {
                if (UnityEngine.Random.Range(0, 100) > 100 - (enemySpawnRate * 0.33f))
                {
                    Instantiate(skeletonEnemy, CoordToWorldPoint(coord) + Vector3.up * 0.5f, Quaternion.identity, enemyParent);
                }
            }
        }
    }

    void PlaceEnemiesInRoom(Room room)
    {
        foreach(Coord coord in room.edgeTiles)
        {
            if (IsFloorTile(coord.x, coord.y - 1) && IsFloorTile(coord.x - 1, coord.y - 1) && IsFloorTile(coord.x + 1, coord.y - 1) 
                && Vector2.Distance(CoordToWorldPoint(coord), CoordToWorldPoint(playerStart)) > 15)
            {
                if (UnityEngine.Random.Range(0, 100) > 100 - enemySpawnRate)
                {
                    Instantiate(skeletonEnemy, CoordToWorldPoint(coord) + Vector3.up * 0.5f, Quaternion.identity, enemyParent);
                }
            }
        }

        foreach(Coord coord in room.tiles)
        {
            if(NoNeighboringTiles(coord.x, coord.y))
            {
                if (UnityEngine.Random.Range(0, 100) > 100 - (enemySpawnRate * 0.1f) 
                    && Vector2.Distance(CoordToWorldPoint(coord), CoordToWorldPoint(playerStart)) > 15)
                {
                    Instantiate(floaterEnemy, CoordToWorldPoint(coord), Quaternion.identity, enemyParent);
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWalls = GetSorroundingWallCount(x, y);
                if(neighborWalls > 4)
                {
                    map[x, y] =  1;
                }
                else if(neighborWalls < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSorroundingWallCount(int x, int y)
    {
        int wallCount = 0;

        for(int neighborX = x - 1; neighborX <= x + 1; neighborX++)
        {
            for (int neighborY = y - 1; neighborY <= y + 1; neighborY++)
            {
                if (IsInMapRange(neighborX, neighborY))
                {
                    if (neighborX != x || neighborY != y)
                    {
                        wallCount += map[neighborX, neighborY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void RandomFillMap()
    {
        if(useRandomSeed)
        {
            seed = System.DateTime.Now.Millisecond.ToString();
        }

        System.Random rand = new System.Random(seed.GetHashCode());

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = rand.Next(0, 100) < randomFillPercent ? 1 : 0;
                }
            }
        }
    }

    void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegions(1);

        foreach(List<Coord> wallRegion in wallRegions)
        {
            if(wallRegion.Count < wallThreshold)
            {
                foreach(Coord tile in wallRegion)
                {
                    map[tile.x, tile.y] = 0;
                }
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0);
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThreshold)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.x, tile.y] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }

        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;
        rooms = survivingRooms.ToArray();
        ConnectClosestRooms(survivingRooms);
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room> ();
        List<Room> roomListB = new List<Room>();

        if(forceAccessibilityFromMainRoom)
        {
            foreach (Room room in allRooms)
            {
                if(room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach(Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if(roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            foreach(Room roomB in roomListB)
            {
                if (roomA == roomB || roomA.IsConnected(roomB))
                {
                    continue;
                }

                for(int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)Mathf.Pow(tileA.x - tileB.x, 2) + (int)Mathf.Pow(tileA.y - tileB.y, 2);

                        if(distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }

            if(possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if(possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if(!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
        List<Coord> line = GetLine(tileA, tileB);
        foreach(Coord c in line) {
            DrawCircle(c, passageWidth);
        }
    }

    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int drawX = c.x + x;
                    int drawY = c.y + y;
                    if (IsInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.x;
        int y = from.y;

        int dx = to.x - from.x;
        int dy = to.y - from.y;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }



    Vector3 CoordToWorldPoint(Coord tile)
    {
        return new Vector3(-width / 2 + .5f + tile.x, -height / 2 + .5f + tile.y, 0);
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.x, tile.y] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.x - 1; x <= tile.x + 1; x++)
            {
                for (int y = tile.y - 1; y <= tile.y + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.y || x == tile.x))
                    {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }


    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    void SetMapTile(int x, int y, int val)
    {
        targetMap.SetTile(new Vector3Int(x - width / 2,
                                         y - height / 2,
                                         0), val == 1 ? fillTile : emptyTile);
    }

    void SetLadderTile(int x, int y)
    {
        ladderMap.SetTile(new Vector3Int(x - width / 2,
                                         y - height / 2,
                                         0), ladderTile);
    }

    void SetPlatformTile(int x, int y)
    {
        platformMap.SetTile(new Vector3Int(x - width / 2,
                                         y - height / 2,
                                         0), platformTile);
    }

    void PaintMapTiles()
    {
        targetMap.ClearAllTiles();
        ladderMap.ClearAllTiles();
        platformMap.ClearAllTiles();

        //Paint regular tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SetMapTile(x, y, map[x, y]);
            }
        }

        //Paint platform tiles
        for(int i = 0; i < platforms.Count; i++)
        {
            for(int c = 0; c < platforms[i].tiles.Length; c++)
            {
                SetPlatformTile(platforms[i].tiles[c].x, platforms[i].tiles[c].y);
            }
        }

        //Paint ladder tiles
        for(int i = 0; i < ladders.Count; i++)
        {
            for (int c = 0; c < ladders[i].tiles.Length; c++)
            {
                SetLadderTile(ladders[i].tiles[c].x, ladders[i].tiles[c].y);
            }
        }

        PaintRooms();
    }

    void PaintRooms()
    {
        //Color Rooms (DEBUG) ===============================
        Color[] colors = new Color[]
        {
            Color.blue,
            Color.red,
            Color.green,
            Color.yellow,
            Color.cyan,
            Color.green,
            Color.grey,
            Color.magenta,
            Color.black
        };
        int colorIndex = 0;

        //Destroy previous tiles
        for(int i = roomParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(roomParent.GetChild(i).gameObject);
        }

        foreach (Room room in rooms)
        {
            //Set the rooms color
            Color roomColor = colors[colorIndex];
            colorIndex++;
            if (colorIndex == colors.Length) colorIndex = 0;

            int lowestX = room.tiles[0].x;
            int highestX = room.tiles[0].x;
            int lowestY = room.tiles[0].y;
            int highestY = room.tiles[0].y;

            foreach (Coord coord in room.tiles)
            {
                if (coord.x > highestX) highestX = coord.x;
                if (coord.x < lowestX) lowestX = coord.x;
                if (coord.y > highestY) highestY = coord.y;
                if (coord.y < lowestY) lowestY = coord.y;

                if (map[coord.x, coord.y] == 1) continue;
                Vector3Int position = new Vector3Int(coord.x - width / 2, coord.y - height / 2, 0);
                targetMap.SetColor(position, roomColor);
                targetMap.SetTileFlags(position, TileFlags.LockColor);
            }

            Vector3 lowest = CoordToWorldPoint(new Coord(lowestX, lowestY));
            Vector3 highest = CoordToWorldPoint(new Coord(highestX, highestY));
            GameObject roomObject = Instantiate(roomPrefab, roomParent);

            Vector3 roomCenter = (lowest + highest) / 2f;
            float scaleX = Vector3.Distance(new Vector3(lowestX, 0, 0), new Vector3(highestX, 0, 0));
            float scaleY = Vector3.Distance(new Vector3(0, lowestY, 0), new Vector3(0, highestY, 0));
            roomObject.transform.position = roomCenter;
            roomObject.transform.localScale = new Vector3(scaleX, scaleY, 0);
        }
    }


    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room() { }

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles)
            {
                for (int x = tile.x - 1; x <= tile.x + 1; x++)
                {
                    for (int y = tile.y - 1; y <= tile.y + 1; y++)
                    {
                        if (x == tile.x || y == tile.y)
                        {
                            if (map[x, y] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if(roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if(roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }

            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public void SetAccessibleFromMainRoom()
        {
            if(!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach(Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}

//Editor Tools
#if UNITY_EDITOR
    [CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapGenerator myScript = (MapGenerator)target;
        if (GUILayout.Button("Generate New Map"))
        {
            myScript.GenerateMap();
        }
    }
}
#endif
