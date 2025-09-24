using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySearchSystem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;   // 검색 입력창
    public Transform buttonParent;      // 버튼들이 들어갈 부모 (Grid Layout Group)
    public GameObject buttonPrefab;     // 버튼 프리팹
    public UnityEngine.UI.Button linearButton;
    public UnityEngine.UI.Button binaryButton;

    private List<Item> items = new List<Item>();
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Start()
    {
        // 아이템 100개 생성 (Item_00 ~ Item_99)
        for (int i = 0; i < 100; i++)
        {
            string name = $"Item_{i:D2}";
            items.Add(new Item(name, Random.Range(1, 10)));
        }

        // 이름순 정렬 (이진 탐색용)
        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        // 버튼 이벤트 등록
        linearButton.onClick.AddListener(OnLinearSearch);
        binaryButton.onClick.AddListener(OnBinarySearch);

        // 처음 실행 시 전체 아이템 버튼 표시
        ShowAllItems();
    }

    private void OnLinearSearch()
    {
        string target = inputField.text;
        Item found = LinearSearch(target);

        ClearButtons();

        if (found != null)
            CreateItemButton(found, $"{found.itemName}");
        else
            CreateMessageButton($"[선형 탐색] {target} 없음");
    }

    private void OnBinarySearch()
    {
        string target = inputField.text;
        Item found = BinarySearch(target);

        ClearButtons();

        if (found != null)
            CreateItemButton(found, $"{found.itemName}");
        else
            CreateMessageButton($"[이진 탐색] {target} 없음");
    }

    // 선형 탐색
    private Item LinearSearch(string target)
    {
        foreach (var item in items)
        {
            if (item.itemName == target)
                return item;
        }
        return null;
    }

    // 이진 탐색
    private Item BinarySearch(string target)
    {
        int left = 0, right = items.Count - 1;
        while (left <= right)
        {
            int mid = (left + right) / 2;
            int cmp = items[mid].itemName.CompareTo(target);

            if (cmp == 0) return items[mid];
            else if (cmp < 0) left = mid + 1;
            else right = mid - 1;
        }
        return null;
    }

    // 전체 아이템 버튼 생성
    private void ShowAllItems()
    {
        ClearButtons();
        foreach (var item in items)
        {
            CreateItemButton(item, $"{item.itemName}");
        }
    }

    // 버튼 생성
    private void CreateItemButton(Item item, string label)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonParent);
        TMP_Text btnText = newButton.GetComponentInChildren<TMP_Text>();
        btnText.text = label;

        Button btn = newButton.GetComponent<Button>();
        btn.onClick.AddListener(() => OnItemButtonClicked(item));

        spawnedButtons.Add(newButton);
    }

    // 검색 실패 시 안내 버튼 생성
    private void CreateMessageButton(string message)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonParent);
        TMP_Text btnText = newButton.GetComponentInChildren<TMP_Text>();
        btnText.text = message;
        spawnedButtons.Add(newButton);
    }

    // 버튼 제거
    private void ClearButtons()
    {
        foreach (var btn in spawnedButtons)
        {
            Destroy(btn);
        }
        spawnedButtons.Clear();
    }

    private void OnItemButtonClicked(Item item)
    {
        Debug.Log($"[상점] {item.itemName} 선택됨! 개수 : {item.quantity}");
    }
}
