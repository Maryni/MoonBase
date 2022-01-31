using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildTypeComponent
{
    None,
    MoonBaseComponent,
    MoonIron,
    MoonSteel
}

public enum BuildType
{
    BuildMoonBase,
    BuildMoonIron,
    BuildMoonSteel
}
public class Building : MonoBehaviour
{

    #region Inspector variables
    
    [SerializeField] private float rateItemCreation;
    [SerializeField] private float offsetItemPlace;
    [SerializeField] private BuildingResourceCreation buildingResourceCreation;

    public float RateResourcesCreation => rateItemCreation;
    public BuildingResourceCreation BuildingResourceCreation => buildingResourceCreation;
    #endregion Inspector variables

    #region private variables

    private Transform transformGetResources;
    private Transform transformSetResources;

    #endregion

    //write logic to get/set recources

    #region Unity functions

    private void Start()
    {
        
    }

    #endregion Unity functions

    #region public functions

    public void SetRecourcesTransforms(Transform transformGet, Transform transformSet)
    {
        transformGetResources = transformGet;
        transformSetResources = transformSet;
    }

    public void SetBuildingResourceCreation(BuildingResourceCreation component)
    {
        buildingResourceCreation = component;
        buildingResourceCreation.SetOffset(offsetItemPlace);
    }
    
    #endregion public functions
}
