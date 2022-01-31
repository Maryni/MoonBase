using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerActions : MonoBehaviour
{
    private UnityAction actionOnTrigger;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Movement>())
        {
            actionOnTrigger?.Invoke(); 
        }
    }

    public void SetActionOnTrigger(UnityAction action)
    {
        actionOnTrigger += action;
    }
}
