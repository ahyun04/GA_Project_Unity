using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 15;
    public int height = 15;

    public int[,] map;
    public Vector2Int start;
    public Vector2Int goal;

    public void Generate()
    {
        map = new int[width, height];
        System.Random rand = new System.Random();

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                int r = rand.Next(0, 100);
                if (r < 20) map[x, y] = 0; 
                else
                {
                    int rr = rand.Next(0, 100);
                    if (rr < 60) map[x, y] = 1; 
                    else if (rr < 90) map[x, y] = 3; 
                    else map[x, y] = 5; 
                }
            }

        start = new Vector2Int(1, 1);
        goal = new Vector2Int(width - 2, height - 2);

        map[start.x, start.y] = 1;
        map[goal.x, goal.y] = 1;
    }

    public class MazeCheck
    {
        static Vector2Int[] dirs = { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

        public static bool CanFinish(int[,] map, Vector2Int start, Vector2Int goal)
        {
            int w = map.GetLength(0);
            int h = map.GetLength(1);

            bool[,] visited = new bool[w, h];
            return DFS(map, start.x, start.y, goal, visited);
        }

        static bool DFS(int[,] map, int x, int y, Vector2Int goal, bool[,] visited)
        {
            if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1)) return false;
            if (map[x, y] == 0 || visited[x, y]) return false;

            visited[x, y] = true;
            if (x == goal.x && y == goal.y) return true;

            foreach (var d in dirs)
                if (DFS(map, x + d.x, y + d.y, goal, visited))
                    return true;
            return false;
        }
    }


}
