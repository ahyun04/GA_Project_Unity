using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MazeGenerator;


public class MazeSystem : MonoBehaviour
{
    public int[,] map;
    public Vector2Int start;
    public Vector2Int goal;

    public MazeGenerator gen;

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