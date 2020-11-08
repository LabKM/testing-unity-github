using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using Coord = UnityEngine.Vector2Int;

public class MapGenerator : MonoBehaviour
{
    
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;
    public GameObject Prison;

    private Vector3 StartPosition;

    public Vector2Int MapSize;
    public Vector3 TileSize;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    
    public int seed = 10;
    [Range(0, 1)]
    public float ObstaclePersent;

    Coord MapCenter;

    //[HideInInspector]
    //public Transform[,] mapArray { set; get; }

    public void GenerateMap()
    {
        ShuffleCoord();

        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        GameObject MapHolder = new GameObject(holderName);
        MapHolder.transform.parent = this.transform;
        StartPosition = new Vector3(-MapSize.x / 2 * TileSize.x, 0, MapSize.y / 2 * TileSize.z);
        MapCenter = new Coord(MapSize.x / 2, MapSize.y / 2);

        bool[,] obstacleMap = new bool[MapSize.x, MapSize.y];
        int obstacleCnt = Math.Min((int)(MapSize.x * MapSize.y * ObstaclePersent), MapSize.x * MapSize.y - 1);
        for (int i = 0; i < obstacleCnt; ++i)
        {
            Coord randomCoord = GetRandomCoord();
            if (obstacleMap[randomCoord.x, randomCoord.y])
            {
                i--;
            }else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = true;
                if (randomCoord != MapCenter && MapIsFullyAccessible(obstacleMap, i + 1))
                {
                    Vector3 obstaclePos = CoordToVector(randomCoord.x, randomCoord.y);
                    GameObject newObstacle = Instantiate<GameObject>(obstaclePrefab, obstaclePos + Vector3.up, Quaternion.identity);
                    newObstacle.transform.parent = MapHolder.transform;
                    newObstacle.transform.name = TileManager.getObstacleName(randomCoord.x, randomCoord.y);
                    //mapArray[randomCoord.x, randomCoord.y] = newObstacle.transform;
                }
                else
                {
                    obstacleMap[randomCoord.x, randomCoord.y] = false;
                    i--;
                }
            }
        } // Obstacle
        if (transform.Find("Prison"))
        {
            DestroyImmediate(transform.Find("Prison").gameObject);
        }
        for (int i = 0; i < 1; ++i)
        {
            Coord randomCoord = GetRandomCoord();
            if (obstacleMap[randomCoord.x, randomCoord.y])
            {
                i--;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = true;
                if (randomCoord != MapCenter && MapIsFullyAccessible(obstacleMap, obstacleCnt + 1))
                {
                    Vector3 obstaclePos = CoordToVector(randomCoord.x, randomCoord.y);
                    GameObject newPrison = Instantiate(Prison, obstaclePos, Quaternion.identity);
                    newPrison.transform.name = "Prison";
                    newPrison.transform.parent = transform;
                }
                else
                {
                    obstacleMap[randomCoord.x, randomCoord.y] = false;
                    i--;
                }
            }
        } // Prison
        for (int i = 0; i < MapSize.x; ++i)
        {
            for (int j = 0; j < MapSize.y; ++j)
            {
                if (!obstacleMap[i, j])
                {
                    Vector3 tilePos = CoordToVector(i, j);
                    GameObject newTile = Instantiate<GameObject>(tilePrefab, tilePos, Quaternion.identity);
                    newTile.name = TileManager.getTileName(i, j);
                    newTile.transform.parent = MapHolder.transform;
                    //mapArray[i, j] = newTile.transform;
                }
            }
        } // Tile
    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlag = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(MapCenter);
        mapFlag[MapCenter.x, MapCenter.y] = true;

        int accessibleTileCount = 1;
        while(queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++ y)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x==0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlag[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlag[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }
        int targetAccessibleTileCount = MapSize.x * MapSize.y - currentObstacleCount;
        return targetAccessibleTileCount == accessibleTileCount;
    }

    public void ShuffleCoord()
    {   
        allTileCoords = new List<Coord>();
        for (int x = 0; x < MapSize.x; ++x)
        {
            for (int y = 0; y < MapSize.y; ++y)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArr<Coord>(allTileCoords.ToArray(), seed));
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    Vector3 CoordToVector(int x, int y)
    {
        return StartPosition + new Vector3(TileSize.x * x, 0, -TileSize.z * y);
    }
}
