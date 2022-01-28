using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Building> buildings = new List<Building>();
    [SerializeField] private Text text;

    private void Start()
    {
        SetAll();
        SetDefaults();
        
        
        
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
    }

    private void SetActions()
    {
        
    }
    private void SetDefaults()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].SetRecourcesTransforms(buildings[i].transform.GetChild(0),buildings[i].transform.GetChild(1));
        }

        if (text == null)
        {
            text = FindObjectOfType<Text>();
        }
    }

    private void StartAllBuildings()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].GetComponent<BuildingRecourceCreation>().StartItemCreationCoroutine(buildings[i].RateResourcesCreation);
        }
    }
}
