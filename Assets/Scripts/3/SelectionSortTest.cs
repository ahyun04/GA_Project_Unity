using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSortTest : MonoBehaviour
{
    
    void Start()
    {
        int[] data = GenerateRandomArray(100);
        StartSlectionSort(data);
        foreach(var item in data)
        {
            Debug.Log(item);
        }
    }

    int[] GenerateRandomArray(int size)
    {
        int[] arr = new int[size];
        System.Random random = new System.Random();
        for (int i = 0; i < size; i++)
        {
            arr[i] = random.Next(0, 10000);
        }
        return arr;
    }

    public static void StartSlectionSort(int[] arr)
    {
        int n = arr.Length;
        for(int i = 0; i < n -1; i++)
        {
            int minIndex = i;
            for(int j = i + 1; j < n; j++)
            {
                if(arr[i] < arr[minIndex])
                {
                    minIndex = j;
                }
            }
            int temp = arr[minIndex];
            arr[minIndex] = arr[i];
            arr[i] = temp;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
