using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    static Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

    public static List<Vector2Int> Dijkstra(int[,] map, Vector2Int start, Vector2Int goal)
    {
        int w = map.GetLength(0), h = map.GetLength(1);

        int[,] dist = new int[w, h];
        Vector2Int?[,] parent = new Vector2Int?[w, h];

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                dist[x, y] = int.MaxValue;

        dist[start.x, start.y] = 0;

        SimplePriorityQueue<Vector2Int> open = new SimplePriorityQueue<Vector2Int>();
        open.Enqueue(start, 0);

        while (open.Count > 0)
        {
            var cur = open.Dequeue();
            if (cur == goal) break;

            foreach (var d in dirs)
            {
                int nx = cur.x + d.x;
                int ny = cur.y + d.y;

                if (nx < 0 || ny < 0 || nx >= w || ny >= h) continue;
                if (map[nx, ny] == 0) continue;

                int cost = map[nx, ny];
                int newDist = dist[cur.x, cur.y] + cost;

                if (newDist < dist[nx, ny])
                {
                    dist[nx, ny] = newDist;
                    parent[nx, ny] = cur;
                    open.Enqueue(new Vector2Int(nx, ny), newDist);
                }
            }
        }
        return Reconstruct(parent, start, goal);
    }

    static List<Vector2Int> Reconstruct(Vector2Int?[,] parent, Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? cur = goal;
        while (cur.HasValue)
        {
            path.Add(cur.Value);
            if (cur.Value == start) break;
            cur = parent[cur.Value.x, cur.Value.y];
        }
        path.Reverse();
        return path;
    }
}

