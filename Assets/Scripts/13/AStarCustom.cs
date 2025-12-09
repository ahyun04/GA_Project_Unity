using System.Collections.Generic;
using UnityEngine;

public class AStarCustom : MonoBehaviour
{
    public int[,] map;
    public MazeSystem maze;
    public List<Vector2Int> enemies = new List<Vector2Int>();

    public class Node
    {
        public Vector2Int pos;
        public int g;
        public int h;
        public int f => g + h;
        public Node parent;

        public Node(Vector2Int p, int g, int h, Node parent)
        {
            this.pos = p;
            this.g = g;
            this.h = h;
            this.parent = parent;
        }
    }

    void Start()
    {
        map = maze.map;

        /*map = new int[,]
        {
            // 0 = wall, 1 = road
            {1,1,1,1,0,0,1},
            {1,0,0,1,1,1,1},
            {1,1,1,1,0,1,1},
            {1,0,1,1,1,1,1},
            {1,1,1,0,1,1,1}
        };

        enemies.Add(new Vector2Int(2, 1));
        enemies.Add(new Vector2Int(4, 4));

        List<Vector2Int> path = AStar(new Vector2Int(0, 0), new Vector2Int(6, 4));

        Debug.Log("==== Result Path ====");
        foreach (var p in path)
            Debug.Log(p);*/
    }

    List<Vector2Int> AStar(Vector2Int start, Vector2Int goal)
    {
        List<Node> open = new List<Node>();
        HashSet<Vector2Int> closed = new HashSet<Vector2Int>();

        Node startNode = new Node(start, 0, H(start, goal), null);
        open.Add(startNode);

        while (open.Count > 0)
        {
            open.Sort((a, b) => a.f.CompareTo(b.f));
            Node current = open[0];

            if (current.pos == goal)
                return Retrace(current);

            open.RemoveAt(0);
            closed.Add(current.pos);

            foreach (var next in Neighbors(current.pos))
            {
                if (closed.Contains(next)) continue;
                if (map[next.x, next.y] == 0) continue; 

                int newG = current.g + 1;
                Node exist = open.Find(n => n.pos == next);

                if (exist == null)
                {
                    Node node = new Node(
                        next,
                        newG,
                        H(next, goal),
                        current
                    );
                    open.Add(node);
                }
                else if (newG < exist.g)
                {
                    exist.g = newG;
                    exist.parent = current;
                }
            }
        }
        return null;
    }

    List<Vector2Int> Retrace(Node end)
    {
        List<Vector2Int> p = new List<Vector2Int>();
        Node cur = end;
        while (cur != null)
        {
            p.Add(cur.pos);
            cur = cur.parent;
        }
        p.Reverse();
        return p;
    }

    List<Vector2Int> Neighbors(Vector2Int p)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        Vector2Int[] dir = {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
        };

        foreach (var d in dir)
        {
            int nx = p.x + d.x;
            int ny = p.y + d.y;
            if (In(nx, ny))
                list.Add(new Vector2Int(nx, ny));
        }
        return list;
    }

    bool In(int x, int y)
    {
        return x >= 0 && y >= 0 &&
               x < map.GetLength(0) &&
               y < map.GetLength(1);
    }


    int H(Vector2Int a, Vector2Int b)
    {
        int h = Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        h += WallNearPenalty(a);
        h += EnemyPenalty(a);

        return h;
    }

    int WallNearPenalty(Vector2Int pos)
    {
        int p = 0;
        Vector2Int[] dirs = {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };

        foreach (var d in dirs)
        {
            int nx = pos.x + d.x;
            int ny = pos.y + d.y;
            if (!In(nx, ny)) continue;

            if (map[nx, ny] == 0)
                p += 2;
        }
        return p;
    }

    int EnemyPenalty(Vector2Int pos)
    {
        int p = 0;
        foreach (var e in enemies)
        {
            int dist = Mathf.Abs(pos.x - e.x) + Mathf.Abs(pos.y - e.y);
            if (dist < 3)
                p += (4 - dist) * 4;  
        }
        return p;
    }

    int CalcHeuristic(Node a, Node b)
    {
        int h = Mathf.Abs(a.pos.x - b.pos.x) + Mathf.Abs(a.pos.y - b.pos.y);

        foreach (var e in enemies)
        {
            Vector2Int ep = e;

            float dist = Vector2.Distance(
                new Vector2(a.pos.x, a.pos.y),
                new Vector2(ep.x, ep.y)
            );

            if (dist < 5f)
                h += 100;
        }
        return h;
    }


    Vector2Int ToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.z)
        );
    }

    Vector3 ToWorld(int x, int y)
    {
        return new Vector3(x, 0, y);
    }

}
