using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
class Stone
{
    public string name;
    public int exp;
    public int price;

    public Stone(string name, int exp, int price)
    {
        this.name = name;
        this.exp = exp;
        this.price = price;
    }
}


public class GreedyCoinSample : MonoBehaviour
{
    int[] cinType = { 50, 40, 10 };
    Stone[] stones =
{
    new Stone("소", 3, 8),
    new Stone("중", 5, 12),
    new Stone("대", 12, 30),
    new Stone("특대", 20, 45),
};

    public Text textLevel;
    public Text textExp;
    public Text textResult;

    public Slider expSlider;
    public Button enhanceButton;

    int currentLevel = 1;
    int currentExp = 0;
    int needExp;
    int totalGold;
    void UpdateUI()
    {
        needExp = 8 * (currentLevel + 1) * (currentLevel + 1);

        textLevel.text = $"+{currentLevel} -> +{currentLevel + 1}";
        textExp.text = $"필요 경험치 {currentExp}/{needExp}";

        expSlider.maxValue = needExp;
        expSlider.value = currentExp;

        enhanceButton.interactable = currentExp >= needExp;
    }
    void ShowResult(Dictionary<string, int> result, int gold)
    {
        textResult.text = "";

        foreach (var r in result)
        {
            textResult.text += $"강화석 {r.Key} x {r.Value}\n";
        }

        textResult.text += $"\n총 가격 : {gold} gold";
    }

    void Start()
    {
        int needExp = NeedExp(2); // 예: 1→2강

        BruteForce(needExp);

        Debug.Log("경험치 낭비 최소");
        Greedy(needExp, (a, b) => a.exp.CompareTo(b.exp));

        Debug.Log("골드 효율 최대");
        Greedy(needExp, (a, b) =>
        {
            float ea = (float)a.exp / a.price;
            float eb = (float)b.exp / b.price;
            return eb.CompareTo(ea);
        });

        Debug.Log("exp 큰 것부터");
        Greedy(needExp, (a, b) => b.exp.CompareTo(a.exp));
    }


    int Countcoins(int amount)
    {
        int count = 0;

        foreach (int c in cinType)
        {
            int use = amount / c;
            count += use;
            amount -= use * c;
        }
        return count;
    }

    int NeedExp(int nextLevel)
    {
        return 8 * nextLevel * nextLevel;
    }
    void BruteForce(int needExp)
    {
        int minGold = int.MaxValue;
        int[] best = new int[4];

        for (int a = 0; a <= needExp / stones[0].exp + 1; a++)
            for (int b = 0; b <= needExp / stones[1].exp + 1; b++)
                for (int c = 0; c <= needExp / stones[2].exp + 1; c++)
                    for (int d = 0; d <= needExp / stones[3].exp + 1; d++)
                    {
                        int exp = a * stones[0].exp +
                                  b * stones[1].exp +
                                  c * stones[2].exp +
                                  d * stones[3].exp;

                        if (exp < needExp) continue;

                        int gold = a * stones[0].price +
                                   b * stones[1].price +
                                   c * stones[2].price +
                                   d * stones[3].price;

                        if (gold < minGold)
                        {
                            minGold = gold;
                            best[0] = a; best[1] = b; best[2] = c; best[3] = d;
                        }
                    }

        Debug.Log($"[BruteForce] 골드:{minGold} / 소:{best[0]} 중:{best[1]} 대:{best[2]} 특대:{best[3]}");
    }
    void Greedy(int needExp, System.Comparison<Stone> sortRule)
    {
        List<Stone> list = new List<Stone>(stones);
        list.Sort(sortRule);

        int remain = needExp;
        Dictionary<string, int> result = new Dictionary<string, int>();

        foreach (var s in list)
        {
            int count = remain / s.exp;
            if (count > 0)
            {
                result[s.name] = count;
                remain -= count * s.exp;
            }
        }

        // 남은 경험치는 강화석 소로 채우기
        if (remain > 0)
        {
            Stone small = stones[0];
            int need = Mathf.CeilToInt((float)remain / small.exp);
            if (!result.ContainsKey(small.name)) result[small.name] = 0;
            result[small.name] += need;
        }

        int totalExp = 0;
        int totalGold = 0;
        foreach (var r in result)
        {
            Stone s = System.Array.Find(stones, x => x.name == r.Key);
            totalExp += s.exp * r.Value;
            totalGold += s.price * r.Value;
        }

        Debug.Log($"[Greedy] exp:{totalExp} gold:{totalGold}");
    }
    public void OnClickBruteForce()
    {
        BruteForce(NeedExp(2));
    }

    public void OnClickGreedy_MinWaste()
    {
        Greedy(NeedExp(2), (a, b) => a.exp.CompareTo(b.exp));
    }

    public void OnClickGreedy_BestEfficiency()
    {
        Greedy(NeedExp(2), (a, b) =>
        {
            float ea = (float)a.exp / a.price;
            float eb = (float)b.exp / b.price;
            return eb.CompareTo(ea);
        });
    }

    public void OnClickGreedy_BigExp()
    {
        Greedy(NeedExp(2), (a, b) => b.exp.CompareTo(a.exp));
    }

    public void OnClickEnhance()
    {
        if (currentExp < needExp)
        {
            Debug.Log("경험치 부족");
            return;
        }

        currentLevel++;
        currentExp = 0;

        textResult.text = "강화 성공!";
        UpdateUI();
    }
}
