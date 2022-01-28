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
    [SerializeField] private  BuildTypeComponent typeComponent;
    [SerializeField] private StoreAction storeAction;
    [SerializeField] private int capacity;
    [SerializeField] private int countCurrentResources;
    
    private UnityAction actionOnTrigger;

    public StoreAction StoreAction => storeAction;

    private void OnTriggerEnter(Collider other)
    {
        actionOnTrigger?.Invoke();
    }

    public void SetActionOnTrigger(UnityAction action)
    {
        actionOnTrigger += action;
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
}
