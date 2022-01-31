using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> itemList;
    [SerializeField] private int capacity;


    public void SetItemToInventory(Item item)
    {
        if (itemList.Count <= capacity)
        {
            itemList.Add(item);
        }
    }
    /// <summary>
    /// Remove last item
    /// </summary>
    public void GetItemFromInventory()
    {
         itemList.RemoveAt(itemList.Count-1);
    }
}
