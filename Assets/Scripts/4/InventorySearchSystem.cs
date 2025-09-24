using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySearchSystem : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;   // �˻� �Է�â
    public Transform buttonParent;      // ��ư���� �� �θ� (Grid Layout Group)
    public GameObject buttonPrefab;     // ��ư ������
    public UnityEngine.UI.Button linearButton;
    public UnityEngine.UI.Button binaryButton;

    private List<Item> items = new List<Item>();
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Start()
    {
        // ������ 100�� ���� (Item_00 ~ Item_99)
        for (int i = 0; i < 100; i++)
        {
            string name = $"Item_{i:D2}";
            items.Add(new Item(name, Random.Range(1, 10)));
        }

        // �̸��� ���� (���� Ž����)
        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        // ��ư �̺�Ʈ ���
        linearButton.onClick.AddListener(OnLinearSearch);
        binaryButton.onClick.AddListener(OnBinarySearch);

        // ó�� ���� �� ��ü ������ ��ư ǥ��
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
            CreateMessageButton($"[���� Ž��] {target} ����");
    }

    private void OnBinarySearch()
    {
        string target = inputField.text;
        Item found = BinarySearch(target);

        ClearButtons();

        if (found != null)
            CreateItemButton(found, $"{found.itemName}");
        else
            CreateMessageButton($"[���� Ž��] {target} ����");
    }

    // ���� Ž��
    private Item LinearSearch(string target)
    {
        foreach (var item in items)
        {
            if (item.itemName == target)
                return item;
        }
        return null;
    }

    // ���� Ž��
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

    // ��ü ������ ��ư ����
    private void ShowAllItems()
    {
        ClearButtons();
        foreach (var item in items)
        {
            CreateItemButton(item, $"{item.itemName}");
        }
    }

    // ��ư ����
    private void CreateItemButton(Item item, string label)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonParent);
        TMP_Text btnText = newButton.GetComponentInChildren<TMP_Text>();
        btnText.text = label;

        Button btn = newButton.GetComponent<Button>();
        btn.onClick.AddListener(() => OnItemButtonClicked(item));

        spawnedButtons.Add(newButton);
    }

    // �˻� ���� �� �ȳ� ��ư ����
    private void CreateMessageButton(string message)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonParent);
        TMP_Text btnText = newButton.GetComponentInChildren<TMP_Text>();
        btnText.text = message;
        spawnedButtons.Add(newButton);
    }

    // ��ư ����
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
        Debug.Log($"[����] {item.itemName} ���õ�! ���� : {item.quantity}");
    }
}
