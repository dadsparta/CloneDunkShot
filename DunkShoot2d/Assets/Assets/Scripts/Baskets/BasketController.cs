using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
     [SerializeField] private float _offset;

     private void Update()
     {
          if (BallStateController.IsPressed)
          {
               Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
               float rotationZ = Mathf.Atan2(worldMousePosition.y, worldMousePosition.x) * Mathf.Rad2Deg;
               transform.rotation = Quaternion.Euler(0f,0f, rotationZ + _offset);
               
               Vector3 localScale = Vector3.one;

               transform.localScale = localScale;
          }
     }

     private void OnMouseDown()
     {
          BallStateController.IsPressed = true;

     }

     private void OnMouseUp()
     {
          BallStateController.IsPressed = false;
     }
}
