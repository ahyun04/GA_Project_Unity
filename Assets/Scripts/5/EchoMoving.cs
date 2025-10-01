using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoMoving : MonoBehaviour
{
    public float speed = 5f;
    public Material normalMaterial;
    public Material echoMaterial;

    private Queue<Vector3> commandQueue = new Queue<Vector3>();
    private Stack<Vector3> moveHistory = new Stack<Vector3>();

    private Renderer rend;
    private bool isRewinding = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!isRewinding)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            if (x != 0 || y != 0)
            {
                // 현재 위치 Stack에 저장
                moveHistory.Push(transform.position);

                // 이동
                Vector3 move = new Vector3(x, y, 0).normalized * speed * Time.deltaTime;
                transform.position += move;

                // Queue에도 기록
                commandQueue.Enqueue(transform.position);
            }
        }

        // Space → Queue 실행
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (commandQueue.Count > 0)
            {
                transform.position = commandQueue.Dequeue();
            }
        }

        // R → Stack 되돌리기
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Rewind());
        }
    }

    IEnumerator Rewind()
    {
        isRewinding = true;
        rend.material = echoMaterial;

        while (moveHistory.Count > 0)
        {
            transform.position = moveHistory.Pop();
            yield return new WaitForSeconds(0.05f);
        }

        rend.material = normalMaterial;
        isRewinding = false;
    }
}
