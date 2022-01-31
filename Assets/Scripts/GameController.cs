using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

    private void Awake()
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
            buildings[i].GetComponent<BuildingRecourceCreation>().SetActionsWhenItemCreated(() => 
                poolManager.GetItemByType(buildings[i].GetComponent<BuildingRecourceCreation>().BuildTypeComponents));
            buildings[i].GetComponent<BuildingRecourceCreation>().SetActionsWhenResourceZero(
                () => texts[i].text = $"Resource are zero in {buildings[i].name}");
            buildings[i].GetComponent<BuildingRecourceCreation>().SetActionsWhenCompleteComponentFull(
                ()=> texts[i].text = $"Store in {buildings[i].name} is full");
            
        }
        //get to inventory
        //set to store
        
        
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
    }

    private void StartAllBuildings()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].GetComponent<BuildingRecourceCreation>().StartItemCreationCoroutine(buildings[i].RateResourcesCreation);
        }
    }

    private void ShowObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void HideObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
