using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static InputManager instance;
    public UnityEvent OnMouseDown;
    public UnityEvent OnMouseDrag;
    public UnityEvent OnMouseUp;

    private void Awake()
    {
        if (!instance) instance = this;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnMouseDrag?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseUp?.Invoke();
    }
}    
