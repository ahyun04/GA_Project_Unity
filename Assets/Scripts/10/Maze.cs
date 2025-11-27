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
    public GameObject playerPrefab;

    private int[,] map;
    private GameObject[,] tiles;

    private Vector2Int start;
    private Vector2Int goal;

    private GameObject player;

    private readonly Vector2Int[] dirs =
    {
        new (1,0), new(-1,0), new(0,1), new(0,-1)
    };

    void Start()
    {
        GenerateMaze();
        SpawnPlayer();
    }

    void Update() { }

    
    public void OnClick_ShowPath()
    {
        ShowShortestPath();
    }

    public void OnClick_AutoMove()
    {
        StartAutoMove();
    }

    public void OnClick_NewMaze() 
    {
        ClearMaze();
        GenerateMaze();
        SpawnPlayer();
    }

    
    void GenerateMaze()
    {
        map = new int[height, width];
        tiles = new GameObject[height, width];

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
                Vector3 pos = new(x, 0, y);
                tiles[y, x] = Instantiate(
                    map[y, x] == 1 ? wallPrefab : floorPrefab,
                    pos, Quaternion.identity, transform
                );
            }
        }
    }

    void ClearMaze()
    {
        foreach (Transform t in transform)
            Destroy(t.gameObject);

        if (player) Destroy(player);
    }

    void SpawnPlayer()
    {
        Vector3 pos = new(start.x, 0.5f, start.y);
        player = Instantiate(playerPrefab, pos, Quaternion.identity);
    }

    void ShowShortestPath()
    {
        List<Vector2Int> path = BFS(start, goal);
        if (path == null) return;

        foreach (var p in path)
        {
            if (p == start || p == goal) continue;
            Vector3 pos = new(p.x, 0.1f, p.y);
            Instantiate(pathPrefab, pos, Quaternion.identity, transform);
        }
    }

    void StartAutoMove()
    {
        List<Vector2Int> path = BFS(start, goal);
        if (path == null) return;

        StopAllCoroutines();
        StartCoroutine(MoveAlong(path));
    }

    IEnumerator MoveAlong(List<Vector2Int> path)
    {
        foreach (var p in path)
        {
            Vector3 targetPos = new(p.x, 0.5f, p.y);
            while (Vector3.Distance(player.transform.position, targetPos) > 0.01f)
            {
                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    targetPos,
                    Time.deltaTime * 3f
                );
                yield return null;
            }
            yield return new WaitForSeconds(0.05f);
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
