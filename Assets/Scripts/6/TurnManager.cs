using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유닛 정보 클래스



public class TurnManager : MonoBehaviour
{
    private List<Unit> units = new List<Unit>();
    private SimplePriorityQueue<Unit> turnQueue = new SimplePriorityQueue<Unit>();
    private int turnCount = 0;

    void Start()
    {
        // 유닛 등록 (이름, 속도)
        units.Add(new Unit("전사", 5));
        units.Add(new Unit("마법사", 7));
        units.Add(new Unit("궁수", 10));
        units.Add(new Unit("도적", 12));

        // 첫 턴은 랜덤으로 진행
        ShuffleAndEnqueueAll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (turnQueue.Count == 0)
            {
                // 4턴까지는 랜덤, 이후부터는 속도 기반으로
                if (turnCount < 4)
                    ShuffleAndEnqueueAll();
                else
                    EnqueueBySpeed();
            }

            Unit current = turnQueue.Dequeue();
            turnCount++;

            Unit target = units[Random.Range(0, units.Count)];
            if (target == current) target = units[(units.IndexOf(current) + 1) % units.Count];

            current.Attack(target);
            Debug.Log($"{turnCount}턴 / {current.name}의 턴입니다.");
        }
    }

    // 랜덤 순서로 큐에 등록
    void ShuffleAndEnqueueAll()
    {
        List<Unit> temp = new List<Unit>(units);
        for (int i = 0; i < temp.Count; i++)
        {
            Unit u = temp[i];
            float randomPriority = Random.Range(0f, 1f);
            turnQueue.Enqueue(u, randomPriority);
        }
    }

    // 속도 기반으로 우선순위 정렬
    void EnqueueBySpeed()
    {
        foreach (var u in units)
        {
            // 속도가 빠를수록 쿨타임이 더 빨리 차도록 설정
            u.cooldown += 1f / u.speed;
            turnQueue.Enqueue(u, u.cooldown);
        }
    }

    public class Unit
    {
        public string name;
        public float speed;
        public float cooldown;
        public int hp;

        public Unit(string name, float speed)
        {
            this.name = name;
            this.speed = speed;
            this.cooldown = 0f;
            this.hp = 100;
        }

        public void Attack(Unit target)
        {
            int damage = Random.Range(10, 30);
            target.hp -= damage;
            Debug.Log($"{name}이(가) {target.name}에게 {damage} 데미지를 입힘! (남은 HP: {target.hp})");
        }
    }
}
