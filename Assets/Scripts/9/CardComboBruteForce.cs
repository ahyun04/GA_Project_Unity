using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CardComboBruteForce : MonoBehaviour
{
    public Button startButton;
    public int costLimit = 15;

    struct CardInfo
    {
        public int damage; public int cost; public int maxCount;
        public CardInfo(int dmg, int c, int max) { damage = dmg; cost = c; maxCount = max; }
    }

    CardInfo quick = new CardInfo(6, 2, 2);   
    CardInfo heavy = new CardInfo(8, 3, 2);   
    CardInfo multi = new CardInfo(16, 5, 1); 
    CardInfo triple = new CardInfo(24, 7, 1); 

    Coroutine runningRoutine;

    void Start()
    {
        if (startButton != null) startButton.onClick.AddListener(OnStartButtonClicked);
    }

    public void OnStartButtonClicked()
    {
        if (runningRoutine != null)
        {
            Debug.Log("[Combo] 이미 실행중입니다.");
            return;
        }
        runningRoutine = StartCoroutine(BruteForceCombosRoutine());
    }

    IEnumerator BruteForceCombosRoutine()
    {
        Debug.Log("[Combo] 브루트포스 탐색 시작");
        Stopwatch sw = new Stopwatch();
        sw.Start();

        int maxDamage = int.MinValue;
        List<string> bestCombos = new List<string>();
        long tryCount = 0;

        for (int q = 0; q <= quick.maxCount; q++)
        {
            for (int h = 0; h <= heavy.maxCount; h++)
            {
                for (int m = 0; m <= multi.maxCount; m++)
                {
                    for (int t = 0; t <= triple.maxCount; t++)
                    {
                        tryCount++;

                        int totalCost = q * quick.cost + h * heavy.cost + m * multi.cost + t * triple.cost;
                        if (totalCost > costLimit) continue; 

                        int totalDamage = q * quick.damage + h * heavy.damage + m * multi.damage + t * triple.damage;

                        if (totalDamage > maxDamage)
                        {
                            maxDamage = totalDamage;
                            bestCombos.Clear();
                            bestCombos.Add(FormatCombo(q, h, m, t, totalCost, totalDamage));
                        }
                        else if (totalDamage == maxDamage)
                        {
                            bestCombos.Add(FormatCombo(q, h, m, t, totalCost, totalDamage));
                        }

                        if (tryCount % 1000 == 0)
                        {
                            yield return null;
                        }
                    }
                }
            }
        }

        sw.Stop();
        Debug.Log($"[Combo] 탐색 완료. 시도수 = {tryCount}, 소요 = {sw.Elapsed.TotalSeconds:F3}초");
        Debug.Log($"[Combo] 최대 데미지 = {maxDamage}");
        Debug.Log("[Combo] 최대 데미지 조합들:");
        foreach (var s in bestCombos)
        {
            Debug.Log(s);
        }

        runningRoutine = null;
    }

    string FormatCombo(int q, int h, int m, int t, int cost, int dmg)
    {
        return $"Quick x{q}, Heavy x{h}, Multi x{m}, Triple x{t}  | Cost={cost}, Damage={dmg}";
    }
}
