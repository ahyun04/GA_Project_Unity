using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [Header("미로 크기 (홀수 추천)")]
    public int width = 15;
    public int height = 15;

    [Header("프리팹 설정")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject pathPrefab;

    private int[,] map;
    private GameObject[,] tiles;

    private Vector2Int start;
    private Vector2Int goal;

    private readonly Vector2Int[] dirs = {
        new(1,0), new(-1,0), new(0,1), new(0,-1)
    };

    void Start()
    {
        GenerateMaze();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 맵 전체 초기화 후 새 미로 생성
            ClearMaze();
            GenerateMaze();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            // 최단 경로 표시
            ShowShortestPath();
        }
    }

    void GenerateMaze()
    {
        map = new int[height, width];
        tiles = new GameObject[height, width];

        // 외곽은 벽으로
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                    map[y, x] = 1;
                else
                    map[y, x] = Random.value < 0.3f ? 1 : 0;
            }
        }

        start = new Vector2Int(1, 1);
        goal = new Vector2Int(width - 2, height - 2);
        map[start.y, start.x] = 0;
        map[goal.y, goal.x] = 0;

        DrawMaze();
    }

    void DrawMaze()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new Vector3(x, 0, y);
                if (map[y, x] == 1)
                    tiles[y, x] = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                else
                    tiles[y, x] = Instantiate(floorPrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    void ClearMaze()
    {
        if (tiles == null) return;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void ShowShortestPath()
    {
        List<Vector2Int> path = BFS(start, goal);
        if (path == null)
        {
            Debug.Log("❌ 경로 없음 - 다시 생성 필요");
            return;
        }

        foreach (var p in path)
        {
            if (p == start || p == goal) continue;
            Vector3 pos = new Vector3(p.x, 0.1f, p.y);
            Instantiate(pathPrefab, pos, Quaternion.identity, transform);
        }
    }

    List<Vector2Int> BFS(Vector2Int start, Vector2Int goal)
    {
        Queue<Vector2Int> q = new();
        Dictionary<Vector2Int, Vector2Int> parent = new();

        q.Enqueue(start);
        parent[start] = start;

        while (q.Count > 0)
        {
            var cur = q.Dequeue();

            if (cur == goal)
            {
                // 역추적
                List<Vector2Int> path = new();
                while (cur != start)
                {
                    path.Add(cur);
                    cur = parent[cur];
                }
                path.Add(start);
                path.Reverse();
                return path;
            }

            foreach (var d in dirs)
            {
                Vector2Int next = cur + d;
                if (next.x < 0 || next.y < 0 || next.x >= width || next.y >= height) continue;
                if (map[next.y, next.x] == 1) continue;
                if (parent.ContainsKey(next)) continue;

                parent[next] = cur;
                q.Enqueue(next);
            }
        }

        return null;
    }
}
