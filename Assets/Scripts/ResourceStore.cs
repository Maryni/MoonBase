using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum StoreAction
{
    Get,
    Set
}
public class ResourceStore : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private BuildTypeComponent typeComponent;
    [SerializeField] private StoreAction storeAction;
    [SerializeField] private int capacity;
    [SerializeField] private int countCurrentResources;

    #endregion Inspector variables

    #region private variables

    private float rateActionWithInventory;
    private UnityAction actionOnTrigger;
    private Coroutine itemActionCoroutine;

    #endregion private variables

    #region properties

    public StoreAction StoreAction => storeAction;
    public BuildTypeComponent BuildTypeComponent => typeComponent;
    public int CountCurrentResources => countCurrentResources;

    #endregion properties

    #region Unity functions

    private void OnTriggerEnter(Collider other)
    {
        ActivateItemActionCoroutine();
    }

    private void OnTriggerExit(Collider other)
    {
        BreakCoroutine();
    }

    #endregion Unity functions

    #region public functions

    public void SetActionOnTrigger(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionOnTrigger += actions[i];;
        }
        
    }
    
    public bool IsHaveResource()
    {
        if (countCurrentResources > 0)
        {
            return true;
        }

        return false;
    }

    public bool IsHaveEnoughtResourcesForCraft(int value)
    {
        if (countCurrentResources >= value)
        {
            return true;
        }

        return false;
    }

    public bool IsHavePlaceForNewResources()
    {
        if (capacity > countCurrentResources)
        {
            return true;
        }

        return false;
    }

    public void SetItemInStore()
    {
        if (IsHavePlaceForNewResources())
        {
            countCurrentResources++;  
        }
    }

    public void GetItemFromStore()
    {
        if (IsHaveResource())
        {
            countCurrentResources--;
        }
    }

    public void SetRateActionWithInventory(float value)
    {
        rateActionWithInventory = value;
    }

    #endregion public functions

    #region private functions

    private void ActivateItemActionCoroutine()
    {
        if (itemActionCoroutine == null)
        {
            itemActionCoroutine = StartCoroutine(ItemAction());
        }
    }

    private IEnumerator ItemAction()
    {
        actionOnTrigger?.Invoke();
        yield return rateActionWithInventory;
        yield return ItemAction();
    }

    private void BreakCoroutine()
    {
        StopCoroutine(itemActionCoroutine);
        itemActionCoroutine = null;
    }
    
    #endregion private functions
    
}
