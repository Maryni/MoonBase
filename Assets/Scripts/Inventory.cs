using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private List<Item> itemList;
    [SerializeField] private int capacity;
    [SerializeField] private Transform inventoryPool;
    [SerializeField] private float rateWithInventoryAction;
    [SerializeField] private float offset;
    [SerializeField] private float maxOffset;

    #endregion Inspector variables

    #region private variables

    private float offsetDefault;

    #endregion private variables

    public float RateInventoryAction => rateWithInventoryAction;
    
    #region public functions

    public void SetItemToInventory(Item item)
    {
        if (itemList.Count <= capacity)
        {
            itemList.Add(item);
            item.transform.parent = inventoryPool;
            item.transform.localPosition = new Vector3(0f,offset,0f);
            if (offset <= maxOffset)
            {
                offset += offsetDefault;
            }
            else
            {
                offset = offsetDefault;
            }
        }
    }
    /// <summary>
    /// Remove last item
    /// </summary>
    public Item GetItemFromInventory()
    {
        int i = itemList.Count - 1;
        itemList[i].gameObject.SetActive(false);
        var obj = itemList[i];
        itemList.RemoveAt(i);
        return obj;
    }


    public void SetInventoryPoolTransform(Transform transform)
    {
        inventoryPool = transform;
    }

    #endregion public functions

    
}
