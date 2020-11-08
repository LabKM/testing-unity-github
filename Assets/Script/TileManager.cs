using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Coord = UnityEngine.Vector2Int;
[RequireComponent(typeof(MapGenerator))]
public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject EnemyPrefab;
    GameObject prison_prefab;
    MapGenerator mapGenerator;

    Coord MapSize;
    Vector3 tile_start_point;
    Vector3 tile_size;

    GameObject player;
    public Coord player_coord { private set;  get; }
    TileContoller[,] tileMap;

    [Range(1, 20)]
    public int SpawnDistance;
    public int AmountEnemy = 30;

    void Awake()
    {
        EnemyPrefab = Resources.Load<GameObject>("Prefab/Enemy");
        prison_prefab = Resources.Load<GameObject>("Prefab/Prison");
        tile_size = new Vector3(2f, 0, 2f);
        mapGenerator = this.GetComponent<MapGenerator>();
        MapSize = mapGenerator.MapSize;
        tile_start_point = new Vector3(-MapSize.x / 2 * tile_size.x, 0, MapSize.y / 2 * tile_size.z);
        tileMap = new TileContoller[MapSize.x, MapSize.y];
        spawnableCoords = new Queue<Coord>();
    }

    void Start()
    {
        Transform mapInst = transform.Find("Generated Map");
        for (int i = 0; i < MapSize.x; ++i) 
            for (int j = 0; j < MapSize.y; ++j)
            {
                Transform tile = mapInst.Find(getTileName(i, j));
                if (tile != null)
                {
                    tileMap[i, j] = tile.GetComponent<TileContoller>();
                }
            }
        //for (int k = 0; k < Amount_Enemy; ++k)
        //{
        //    GameObject enemy_inst = (GameObject)Instantiate<GameObject>(EnemyPrefab, Vector3.zero, Quaternion.identity);
        //    enemy_inst.name = "Enemy_" + k.ToString();
        //    enemy_inst.SetActive(false);
        //    enemyList.Add(enemy_inst);
        //}
        player = GameObject.FindGameObjectWithTag("Player");
        player_coord = VectorToCoord(player.transform.position);
        StartCoroutine(findRandomTilesNearPlayer());
        StartCoroutine(spawnEnemy());
        //Instantiate<GameObject>(prison_prefab,
        //transIndexToPosition(getIndex(player_index, UnityEngine.Random.Range(-Amount_Tile_Show_Set_x/2, Amount_Tile_Show_Set_x / 2),
        //UnityEngine.Random.Range(-Amount_Tile_Show_Set_z/2, Amount_Tile_Show_Set_z/2))),
        //Quaternion.identity);
    }

    public float Spawn_Enemy_Per_Sec = 5f;

    IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(2.0f);
        while (true)
        {
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCount < AmountEnemy)
            {
                GameObject enemy = Instantiate(EnemyPrefab, Vector3.zero, Quaternion.identity);
                enemy.SetActive(false);
                Enemy enemy_componet = enemy.GetComponent<Enemy>();
                Coord rand_index = getRandomTileNearPlayer();
                StartCoroutine(tileMap[rand_index.x, rand_index.y].Flickering(enemy_componet));
            }
            yield return new WaitForSeconds(1f / Spawn_Enemy_Per_Sec);
        }
    }

    Queue<Coord> spawnableCoords;
    IEnumerator findRandomTilesNearPlayer()
    {
        while (true)
        {
            player_coord = VectorToCoord(player.transform.position);
            Coord startCoord = new Coord(Math.Max(0, player_coord.x - SpawnDistance), Math.Max(0, player_coord.y - SpawnDistance));
            Coord endCoord = new Coord(Math.Min(MapSize.x, player_coord.x + SpawnDistance), Math.Min(MapSize.y, player_coord.y + SpawnDistance));
            //스폰 맵 플레이어 주변 false로 초기화
            Queue<Coord> searchQueue = new Queue<Coord>();
            List<Coord> spawnableTile = new List<Coord>();
            bool[,] spawnMap = new bool[endCoord.x - startCoord.x, endCoord.y - startCoord.y];
            spawnMap[player_coord.x - startCoord.x, player_coord.y - startCoord.y] = true;
            searchQueue.Enqueue(player_coord);
            if (tileMap[player_coord.x, player_coord.y] == null)
            {
                Debug.Log("Player Coord have wrong");
                Debug.Log(player_coord);
            }
            else
            {
                spawnableTile.Add(player_coord);
            }

            while (searchQueue.Count > 0 && spawnableTile.Count < 10)
            {
                Coord tile = searchQueue.Dequeue();
                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        Coord current = new Coord(tile.x + x, tile.y + y);
                        if (x == 0 || y == 0)
                        {
                            if (current.x >= startCoord.x && current.x < endCoord.x && current.y >= startCoord.y && current.y < endCoord.y)
                            {
                                if (!spawnMap[current.x - startCoord.x, current.y - startCoord.y] && tileMap[current.x, current.y] != null)
                                {
                                    spawnMap[current.x - startCoord.x, current.y - startCoord.y] = true;
                                    searchQueue.Enqueue(new Coord(current.x, current.y));
                                    spawnableTile.Add(new Coord(current.x, current.y));
                                }
                            }
                        }
                    }
                }
            }
            spawnableCoords = new Queue<Coord>(Utility.ShuffleArr<Coord>(spawnableTile.ToArray(), (int)(UnityEngine.Random.value * player_coord.x * player_coord.y)));
            yield return new WaitForSeconds(0.5f);
        }
    }

    Coord getRandomTileNearPlayer()
    {
        if (spawnableCoords.Count > 0)
        {
            Coord tile = spawnableCoords.Dequeue();
            spawnableCoords.Enqueue(tile);
            return tile;
        }
        else
        {
            findRandomTilesNearPlayer();
            Coord tile = spawnableCoords.Dequeue();
            spawnableCoords.Enqueue(tile);
            return tile;
        }
    }

    public Coord VectorToCoord(Vector3 v3)
    {
        Vector3 LeftTopSide = tile_start_point;
        LeftTopSide.x -= tile_size.x / 2;
        LeftTopSide.z += tile_size.z / 2;
        Vector3 FromLeftTopSide = v3 - LeftTopSide;

        return new Coord( (int)(Math.Abs(FromLeftTopSide.x) / tile_size.x ), (int)(Math.Abs(FromLeftTopSide.z) / tile_size.z ) );
    }

    public static string getTileName(int x, int y)
    {
        return "Tile_" + x.ToString() + '_' + y.ToString();
    }
    public static string getObstacleName(int x, int y)
    {
        return "Obstacle_" + x.ToString() + "_" + y.ToString();
    }

    Coord getRandomCoordFrom(Coord center, int x, int y)
    {
        center.x -= UnityEngine.Random.Range(-x, x);
        center.y -= UnityEngine.Random.Range(-y, y);
        return center;
    }
    void configRadar()
    {

    }
}
