using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrugDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region Inspectora variables

    [SerializeField] private RectTransform rectTransform;
    
    #endregion Inspectora variables

    #region private variables

    
    private Vector2 startTransform;
    private Vector2 endTransform;
    private UnityAction actionOnStartDrug;
    private UnityAction actionOnEndDrug;
    private UnityAction actionOnDraging;
    private bool drugStarted;

    #endregion private variables

    #region Unity functions

    private void Start()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        startTransform = rectTransform.anchoredPosition;
        endTransform = Vector2.zero;
        Debug.Log($"rectTransform = {rectTransform.name} | startTransform = {startTransform} | endTransform = {endTransform}");
    }

    private void FixedUpdate()
    {
        if (drugStarted)
        {
            actionOnDraging?.Invoke();
        }
        
    }

    #endregion Unity functions

    #region public functions

    public Vector2 GetStartPosition()
    {
        if (startTransform != Vector2.zero)
        {
            return startTransform;
        }
        else
            return Vector2.zero;
    }

    public Vector2 GetEndPosition()
    {
        if (endTransform != Vector2.zero)
        {
            return endTransform;
        }
        else
            return Vector2.zero;
    }

    public void SetActionToStartDrug(UnityAction action)
    {
        actionOnStartDrug += action;
    }

    public void SetActionToEndDrug(UnityAction action)
    {
        actionOnEndDrug += action;
    }

    public void SetActionToDragging(UnityAction action)
    {
        actionOnDraging += action;
    }

    //Down functions was called in EventTrigger component
    public void OnEndDragFunction()
    {
        endTransform = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = startTransform;
        actionOnEndDrug?.Invoke();
        drugStarted = false;
    }

    public void OnBeginDragFunction()
    {
        drugStarted = true;
        actionOnStartDrug?.Invoke();
    }

    public void OnDragFunction(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
        endTransform = rectTransform.anchoredPosition;
    }

    #endregion public functions

    #region private functions

    private IEnumerator Dragging()
    {
        actionOnDraging?.Invoke();
        yield return new WaitForFixedUpdate();
    }

    #endregion private functions

    #region Interface realize

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrug");
        OnBeginDragFunction();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrug");
        OnDragFunction(eventData);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrug");
        OnEndDragFunction();
    }

    #endregion Interface realize
}