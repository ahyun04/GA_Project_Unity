using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� Ŭ����



public class TurnManager : MonoBehaviour
{
    private List<Unit> units = new List<Unit>();
    private SimplePriorityQueue<Unit> turnQueue = new SimplePriorityQueue<Unit>();
    private int turnCount = 0;

    void Start()
    {
        // ���� ��� (�̸�, �ӵ�)
        units.Add(new Unit("����", 5));
        units.Add(new Unit("������", 7));
        units.Add(new Unit("�ü�", 10));
        units.Add(new Unit("����", 12));

        // ù ���� �������� ����
        ShuffleAndEnqueueAll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (turnQueue.Count == 0)
            {
                // 4�ϱ����� ����, ���ĺ��ʹ� �ӵ� �������
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
            Debug.Log($"{turnCount}�� / {current.name}�� ���Դϴ�.");
        }
    }

    // ���� ������ ť�� ���
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

    // �ӵ� ������� �켱���� ����
    void EnqueueBySpeed()
    {
        foreach (var u in units)
        {
            // �ӵ��� �������� ��Ÿ���� �� ���� ������ ����
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
            Debug.Log($"{name}��(��) {target.name}���� {damage} �������� ����! (���� HP: {target.hp})");
        }
    }
}
