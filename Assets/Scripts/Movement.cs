using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    #region Inspector variables

#pragma warning disable
    [SerializeField] private float modSpeed;
    //[SerializeField] private Text text;
#pragma warning restore

    #endregion Inspector variables

    #region private variables

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 direction;
    private GameObject player;
    private Rigidbody rig;
    private Coroutine jumpCoroutine;
    private bool inJump;
    private DrugDrop drugDrop;

    #endregion private variables

    #region Unity functions

    #endregion Unity functions

    #region public functions

    public void SetDrugDrop(DrugDrop drugDrop)
    {
        this.drugDrop = drugDrop;
        startPos = drugDrop.GetStartPosition();
        if (player == null)
        {
            player = gameObject;
        }
        if (rig == null)
        {
            rig = player.GetComponent<Rigidbody>();
        } 
    }

    public void Move()
    {
        if (drugDrop != null)
        {
            GetPosition();
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                direction = new Vector3(endPos.x - startPos.x, 0, endPos.y - startPos.y).normalized;
                RotateOnDirection();
                rig.velocity = direction * modSpeed;
                direction = Vector2.zero;
            }
        }
        else
        {
            Debug.LogError("DrugDrop = null");
        }
    }

    #endregion public functions

    #region private functions

    private void GetPosition()
    {
        endPos = drugDrop.GetEndPosition();
    }

    private void RotateOnDirection()
    {
        player.transform.rotation = Quaternion.LookRotation(direction);
    }

    #endregion private functions
}