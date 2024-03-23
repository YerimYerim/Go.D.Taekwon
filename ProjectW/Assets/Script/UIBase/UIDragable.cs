using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 savedPosition = Vector3.zero;

    private Action<PointerEventData> _actionFail;
    private Action<PointerEventData> _actionSuccess;
    private AdjustIsSuccess _adjustIsSuccess;
    private Action<PointerEventData> _onDragging;
    
    private RectTransform rectTransform;
    private Canvas _canvas;
    
    public delegate bool AdjustIsSuccess(PointerEventData eventData);


    private void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void InitDragSuccessCondition( Action<PointerEventData> actionFail, Action<PointerEventData> actionSuccess, AdjustIsSuccess adjustIsSuccess, Action<PointerEventData> onDragging)
    {
        _adjustIsSuccess = adjustIsSuccess;
        _actionFail = actionFail;
        _actionSuccess = actionSuccess;
        _onDragging = onDragging;
    }
    
    public virtual void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        _onDragging?.Invoke(eventData);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        savedPosition = transform.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (_adjustIsSuccess?.Invoke(eventData) == true)
        {
            _actionSuccess?.Invoke(eventData);   
        }
        else
        {
            _actionFail?.Invoke(eventData);
        }
    }

    public void MoveReset()
    {
        transform.position = savedPosition;
    }
}
