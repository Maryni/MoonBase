using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Building> buildings;
    [SerializeField] private List<Text> texts;
    [SerializeField] private GameObject textPanelGameObject;
    [SerializeField] private DrugDrop drugDrop;
    [SerializeField] private Movement movement;
    [SerializeField] private PoolManager poolManager;

    private void Start()
    {
        SetAll();
        SetDefaults();
        SetActions();


        StartAllBuildings();
    }

    [ContextMenu("SetBuildings")]
    private void SetAll()
    {
        Building[] tempList = FindObjectsOfType<Building>();
        for (int i = 0; i < tempList.Length; i++)
        {
            if (!buildings.Contains(tempList[i]))
            {
                buildings.Add(tempList[i]);  
            } 
        }

        for (int i = 0; i < textPanelGameObject.transform.childCount; i++)
        {
            if (!texts.Contains(textPanelGameObject.transform.GetChild(i).GetComponent<Text>()))
            {
                texts.Add(textPanelGameObject.transform.GetChild(i).GetComponent<Text>());
            }
        }
    }

    private void SetActions()
    {
        drugDrop.SetActionToDragging(() => movement.Move());

        for (int i = 0; i < buildings.Count; i++)
        {
          SetBuildingActions(i);
          SetResourceStoreAction(i);
        }
    }
    private void SetDefaults()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].SetRecourcesTransforms(buildings[i].transform.GetChild(0),buildings[i].transform.GetChild(1));
        }

        if (poolManager == null)
        {
            poolManager = FindObjectOfType<PoolManager>();
        }

        if (movement == null)
        {
            movement = FindObjectOfType<Movement>();
        }
        
        if (drugDrop == null)
        {
            drugDrop = FindObjectOfType<DrugDrop>();
            movement.SetDrugDrop(drugDrop);
        }

        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].SetBuildingResourceCreation(buildings[i].GetComponent<BuildingResourceCreation>());
            buildings[i].BuildingResourceCreation.SetRateToStores(movement.GetComponent<Inventory>().RateInventoryAction);
        }
        movement.GetComponent<Inventory>().SetInventoryPoolTransform(movement.transform.GetChild(0));
    }

    private void StartAllBuildings()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].BuildingResourceCreation.StartItemCreationCoroutine(buildings[i].RateResourcesCreation);
        }
    }

    private void ResourcesIsZero(int index)
    {
        texts[index].text = $"Resource are zero in {buildings[index].name}";
    }

    private void FullStores(int index)
    {
        texts[index].text = $"Store in {buildings[index].name} is full";
    }

    private void ClearText(int index)
    {
        texts[index].text = "";
    }

    private void SetBuildingActions(int index)
    {
        buildings[index].BuildingResourceCreation.SetActionsWhenItemCreated(
            () => buildings[index].BuildingResourceCreation.PlaceItemOnStore(poolManager.GetItemByType(buildings[index].BuildingResourceCreation.BuildTypeComponents[0])),
            () => buildings[index].BuildingResourceCreation.ResourceStoreSet.GetItemFromStore(),
            () => buildings[index].BuildingResourceCreation.ResourceStoreSet.GetItemFromStore(),
            () => ClearText(index));
        buildings[index].BuildingResourceCreation.SetActionsWhenResourceZero(
            () => ResourcesIsZero(index));
        buildings[index].BuildingResourceCreation.SetActionsWhenCompleteComponentFull(
            () => FullStores(index));
    }

    private void SetResourceStoreAction(int index)
    {
        buildings[index].BuildingResourceCreation.ResourceStoreGet.SetActionOnTrigger(
                () => buildings[index].BuildingResourceCreation.ResourceStoreGet.GetItemFromStore(),
                () => movement.GetComponent<Inventory>().SetItemToInventory(buildings[index].BuildingResourceCreation.GetLastItem())
                );

        buildings[index].BuildingResourceCreation.ResourceStoreSet.SetActionOnTrigger(
                () => buildings[index].BuildingResourceCreation.ResourceStoreSet.SetItemInStore(),
                () => buildings[index].BuildingResourceCreation.PlaceItemOnStore(movement.GetComponent<Inventory>().GetItemFromInventory())  
                );
    }
}
