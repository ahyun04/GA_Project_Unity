using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MazeGenerator;


public class MazeSystem : MonoBehaviour
{
    public MazeGenerator generator;
    public AStarCustom astar;
    public int[,] map;
    public Vector2Int start;
    public Vector2Int goal;

    public MazeGenerator gen;
   
    public GameObject enemyPrefab;
    public int enemyCount = 5;

    public void BuildMaze()
    {
        generator.Generate();
        map = generator.map;
        astar.map = map;

        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2Int pos = GetRandomEmptyPosition();

            Vector3 worldPos = new Vector3(pos.x, 1, pos.y);

            Instantiate(enemyPrefab, worldPos, Quaternion.identity);
        }
    }

    Vector2Int GetRandomEmptyPosition()
    {
        int x, y;

        while (true)
        {
            x = Random.Range(0, map.GetLength(0));
            y = Random.Range(0, map.GetLength(1));

            if (map[x, y] == 0)   // 0 = 길 (벽이 아님)
                return new Vector2Int(x, y);
        }
    }
    void Start()
    {
        while (true)
        {
            gen.Generate();
            if (MazeCheck.CanFinish(gen.map, gen.start, gen.goal))
            {
                Debug.Log("탈출 가능한 맵 생성 완료!");
                break;
            }
            Debug.Log("탈출 불가! 다시 생성...");
        }
    }
}