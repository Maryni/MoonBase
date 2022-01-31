using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private List<GameObject> itemsExample;
    [SerializeField] private List<GameObject> itemsPool;
    [SerializeField] private Transform parentItemsPoolTransform;
    [SerializeField] private int countDefaultItemSpawn;

    #endregion Inspector variables

    #region Unity functions

    private void Start()
    {
        SetPoolAtCount();
    }

    #endregion Unity functions
    
    #region public functions

    public Item GetItemByType (BuildTypeComponent typeComponent)
    {
        for (int i = 0; i < itemsPool.Count; i++)
        {
            if (itemsPool[i].GetComponent<Item>().BuildTypeComponent == typeComponent && !itemsPool[i].activeSelf)
            {
                return itemsPool[i].GetComponent<Item>();
            }
        }
        
        return SetItemInPoolOnce(typeComponent);
    }

    #endregion public functions
    
    #region private functions

    private void SetPoolAtCount()
    {
        for (int i = 0; i < itemsExample.Count; i++)
        {
            for (int j = 0; j < countDefaultItemSpawn; j++)
            {
                var obj = Instantiate(itemsExample[i],parentItemsPoolTransform);
                obj.gameObject.SetActive(false);
                itemsPool.Add(obj);
            }  
        }
    }

    private Item SetItemInPoolOnce(BuildTypeComponent typeComponent)
    {
        for (int i = 0; i < itemsExample.Count; i++)
        {
            if (itemsExample[i].GetComponent<Item>().BuildTypeComponent == typeComponent)
            {
                var obj = Instantiate(itemsExample[i],parentItemsPoolTransform);
                obj.gameObject.SetActive(false); 
                itemsPool.Add(obj);
            }
        }

        return itemsPool[itemsPool.Count-1].GetComponent<Item>();

    }

    #endregion private functions
    


    
}
