using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AutoMove : MonoBehaviour
{
    public MazeSystem maze; 
    public Pathfinder pathfinder;

    public void StartMove()
    {
        int[,] map = maze.map;
        Vector2Int start = maze.start;
        Vector2Int goal = maze.goal;

        if (map == null)
        {
            Debug.LogError("맵이 아직 생성되지 않았습니다!");
            return;
        }

        var path = Pathfinder.Dijkstra(map, start, goal);

        if (path == null) Debug.Log("경로 없음");
        else
        {
            Debug.Log($"경로 길이: {path.Count}");
        }
    }
}

