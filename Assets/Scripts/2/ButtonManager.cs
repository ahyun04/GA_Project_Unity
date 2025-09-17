using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public ArrayInventory arrayInventory;
    public ListInventory listInventory;

    public bool useListInventory = true;

    public void AddPotion()
    {
        if (useListInventory)
            listInventory.AssItem("Potion");
        else
            arrayInventory.AddItem("Potion");
    }

    public void AddSword()
    {
        if (useListInventory)
            listInventory.AssItem("Sword");
        else
            arrayInventory.AddItem("Sword");
    }

    public void RemovePotion()
    {
        if (useListInventory)
            listInventory.RemoveItem("Potion");
        else
            arrayInventory.RemoveItem("Potion");
    }

    public void RemoveSword()
    {
        if (useListInventory)
            listInventory.RemoveItem("Sword");
        else
            arrayInventory.RemoveItem("Sword");
    }

    public void PrintInven()
    {
        if (useListInventory)
            listInventory.PrintInventory();
        else
            arrayInventory.PrintInventory();
    }

    public void RemoveAll()
    {
        if (useListInventory)
        {
            listInventory.RemoveAllItems("Potion");
            listInventory.RemoveAllItems("Potion");
        }
            
        else
            Debug.Log("�迭 �κ��丮�� ���� ������ �����մϴ�.");
    }

}
