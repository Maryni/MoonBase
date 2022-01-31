using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class BuildingResourceCreation : MonoBehaviour
{

   #region Inspector variables

   [SerializeField] private List<BuildTypeComponent> buildTypeComponents;
   [SerializeField] private List<ResourceStore> resourceStores;
   [SerializeField] private List<Item> itemsWhichStoreHave;
   [Header("Settings"),SerializeField] private int countResourceUsingInCreation = 1;
   [SerializeField]  private int countItemCreatePerTime = 1;
   
   #endregion
   
   #region private variables

   private float offset;
   private float offsetDefault;
   private float maxOffset = 10f;
   private bool canBuildComponent = false;
   private Coroutine itemCreationCoroutine;
   private UnityAction actionWhenItemCreated;
   private UnityAction actionWhenCompleteComponentStoreFull;
   private UnityAction actionWhenRecourceGetZero;

   #endregion private variables

   #region properties

   public List<BuildTypeComponent> BuildTypeComponents => buildTypeComponents;
   public List<ResourceStore> ResourceStores => resourceStores;
   public ResourceStore ResourceStoreGet => resourceStores[0];
   public ResourceStore ResourceStoreSet => resourceStores[1];
   public int CountResourceInStoreGet => ResourceStoreGet.CountCurrentResources;
   public int CountResourceInStoreSet => ResourceStoreSet.CountCurrentResources;

   #endregion properties

   #region private functions

   private IEnumerator ItemCreation(float rate)
   {
      yield return new WaitForSeconds(rate);
      IsCanBuildComponents();

      if (canBuildComponent)
      {
         actionWhenItemCreated?.Invoke();
      }


      IsIsHaveEnoughtResourcesForCraft();
      IsHavePlaceForNewResources();

      yield return ItemCreation(rate);
   }

   private void IsCanBuildComponents()
   {
      for (int i = 0; i < resourceStores.Count; i++)
      {
         if (resourceStores[i].IsHaveEnoughtResourcesForCraft(countResourceUsingInCreation))
         {
            canBuildComponent = true;
         }
      }
   }

   private void IsIsHaveEnoughtResourcesForCraft()
   {
      for (int i = 0; i < resourceStores.Count; i++)
      {
         if (!resourceStores[i].IsHaveEnoughtResourcesForCraft(countResourceUsingInCreation))
         {
            actionWhenRecourceGetZero?.Invoke();
         }
      }
   }

   private void IsHavePlaceForNewResources()
   {
      for (int i = 0; i < resourceStores.Count; i++)
      {
         if (!resourceStores[i].IsHavePlaceForNewResources())
         {
            actionWhenCompleteComponentStoreFull?.Invoke();
         }
      }
   }
   
   private void ShowObject(GameObject obj, bool needChangeTransform)
   {
      if (needChangeTransform)
      {
         obj.transform.position = ResourceStoreGet.transform.position;
         obj.transform.localPosition = new Vector3(0,offset,0f);
         offset += offsetDefault;
         if (offset > maxOffset)
         {
            offset = offsetDefault;
         }
      }
      obj.SetActive(true);
   }

   private void HideObject(GameObject obj)
   {
      obj.SetActive(false);
   }
   
   #endregion private functions
   
   #region public functions

   public void StartItemCreationCoroutine(float rateCreation)
   {
      if (itemCreationCoroutine == null)
      {
         itemCreationCoroutine = StartCoroutine(ItemCreation(rateCreation));
      }
   }

   public void StopItemCreationCoroutine()
   {
      StopCoroutine(itemCreationCoroutine);
      itemCreationCoroutine = null;
   }

   public void SetActionsWhenItemCreated(params UnityAction[] actions)
   {
      for (int i = 0; i < actions.Length; i++)
      {
         actionWhenItemCreated += actions[i];
      }
   }
   
   public void SetActionsWhenResourceZero(params UnityAction[] actions)
   {
      for (int i = 0; i < actions.Length; i++)
      {
         actionWhenRecourceGetZero += actions[i];
      }
   }
   
   public void SetActionsWhenCompleteComponentFull(params UnityAction[] actions)
   {
      for (int i = 0; i < actions.Length; i++)
      {
         actionWhenCompleteComponentStoreFull += actions[i];
      }
   }

   public void PlaceItemOnStore(Item item)
   {
      for (int i = 0; i < resourceStores.Count; i++)
      {
         if (resourceStores[i].StoreAction == StoreAction.Set)
         {
            if (resourceStores[i].BuildTypeComponent != BuildTypeComponent.None)
            {
               resourceStores[i].SetItemInStore();
            }
            else
            {
               GetItemFromStore(); 
            }
         }

         if (resourceStores[i].StoreAction == StoreAction.Get)
         {
            resourceStores[i].SetItemInStore();
            item.gameObject.transform.parent = resourceStores[i].transform;
         }
      }
      itemsWhichStoreHave.Add(item);
      ShowObject(item.gameObject,true);
   }

   public void GetItemFromStore()
   {
      for (int i = 0; i < resourceStores.Count; i++)
      {
         if (resourceStores[i].StoreAction == StoreAction.Get)
         {
            resourceStores[i].SetItemInStore();
         }
      }
   }

   public void SetOffset(float value)
   {
      offset = value;
      offsetDefault = offset;
   }

   public Item GetLastItem()
   {
      var obj = itemsWhichStoreHave[itemsWhichStoreHave.Count - 1];
      itemsWhichStoreHave.RemoveAt(itemsWhichStoreHave.Count - 1);
      return obj;
   }

   public void SetRateToStores(float rate)
   {
      for (int i = 0; i < resourceStores.Count; i++)
      {
         resourceStores[i].SetRateActionWithInventory(rate);
      }
   }
   
   #endregion public functions
   
}
