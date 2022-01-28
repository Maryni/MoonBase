using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class BuildingRecourceCreation : MonoBehaviour
{

   #region Inspector variables

   [SerializeField] private List<BuildTypeComponent> buildTypeComponents;

   #endregion
   
   #region private variables
   
   //[SerializeField] private int countComponentCompleted;
   [SerializeField] private List<ResourceStore> resourceStores;

   [Header("Settings"),SerializeField] private int countResourceUsingInCreation = 1;
   [SerializeField]  private int countItemCreatePerTime = 1;

   private bool canBuildComponent = false;
   private Coroutine itemCreationCoroutine;
   private UnityAction actionWhenItemCreated;
   private UnityAction actionWhenCompleteComponentStoreFull;
   private UnityAction actionWhenRecourceGetZero;

   #endregion private variables

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
   
   #endregion private functions
   
   #region public functions

   public void StartItemCreationCoroutine(float rateCreation)
   {
      if (itemCreationCoroutine == null)
      {
         itemCreationCoroutine = StartCoroutine(ItemCreation(rateCreation));
      }
   }

   
   // public void SetCapacity(int value)
   // {
   //    capacity = value;
   // }
   //
   // public void TakeComponents(int value)
   // {
   //    countComponentCompleted -= value;
   // }
   //
   // public void IncreasedCountComponentCompleted(int value)
   // {
   //    countComponentCompleted += value;
   // }

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
   
   #endregion public functions
   
}
